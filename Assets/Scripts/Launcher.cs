using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] int dragButton;
    [Header("Settings")]
    [SerializeField] Transform launchPoint;
    [SerializeField] float launchSpin;
    [SerializeField] float launchPower;
    [SerializeField] float maxDrag;
    [SerializeField] float useCooldown;
    [Header("Resources")]
    [SerializeField] GameObject anvilPrefab;
    [SerializeField] Transform hammer;
    [SerializeField] Transform shadowHammer;
    private GameObject instructionImage;
    private GameObject instructionImage2;
    private bool hasUserRightClickedDragged = false; // Flag 2 check if the user has dragged for first time
    [SerializeField] private float dragThreshold = 5.0f; // Threshold for drag detection
    private bool isRightClickDragging = false;
    private Vector3 initialRightClickPosition;

    [Header("Animation Settings")]
    [SerializeField] private float hammerRotationSpeed = 2.0f; // Control the speed of hammer rotation

    private GameObject trajectoryArrow; // Variable to hold the reference to the trajectory arrow



    private Vector3 previousLaunch;
    SpriteRenderer anvilVisual;
    Collider2D triggerArea;

    float nextUsable = 0f;
    bool dragging = false;

    void Start()
    {
        triggerArea = gameObject.GetComponent<Collider2D>();
        anvilVisual = GetComponent<SpriteRenderer>();
        instructionImage = GameObject.Find("instructions-1");
        instructionImage2 = GameObject.Find("instructions-2");

        trajectoryArrow = GameObject.FindGameObjectWithTag("arrow");
        if (trajectoryArrow != null)
        {
            trajectoryArrow.SetActive(false); // Hide the arrow initially
        }
        else
        {
            UnityEngine.Debug.LogError("Trajectory arrow not found. Make sure it's tagged correctly.");
        }

    }

    public void Launch(Vector3 launchForce, float angularForce, GameObject projectile)
    {
        if (projectile == null) { UnityEngine.Debug.Log("Null projectile"); return; }
        GameObject spawnedProjectile = Instantiate(projectile, launchPoint.position, Quaternion.identity);
        Rigidbody2D physicsBody = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (physicsBody == null) { Destroy(spawnedProjectile); UnityEngine.Debug.Log("Projectile missing rigidbody2D"); return; }
        physicsBody.AddForce(launchForce, ForceMode2D.Impulse);
        physicsBody.angularVelocity = angularForce / physicsBody.mass;

        nextUsable = Time.time + useCooldown;
    }

    private Vector3 CalculateLaunchVector()
    {
        Vector3 offset = transform.position - Vision.instance.GetMouseWorldPosition();
        return offset.normalized * Mathf.Clamp(offset.magnitude / maxDrag, 0f, 1f) * maxDrag;
    }

    bool CanUse()
    {
        return triggerArea.OverlapPoint(Vision.instance.GetMouseWorldPosition()) && Time.time > nextUsable;
    }

    void Update()
    {
        anvilVisual.enabled = Time.time > nextUsable;
        shadowHammer.gameObject.SetActive(dragging);

        float leftmostX = triggerArea.bounds.min.x;

        // Right Mouse Button Down
        if (Input.GetMouseButtonDown(1))
        {
            initialRightClickPosition = Input.mousePosition;
            isRightClickDragging = false;
        }

        // Right Mouse Button Held Down
        if (Input.GetMouseButton(1))
        {
            if (!isRightClickDragging && Vector3.Distance(initialRightClickPosition, Input.mousePosition) > dragThreshold)
            {
                isRightClickDragging = true;

                // Hide instructions-2 on first right-click drag
                if (!hasUserRightClickedDragged && instructionImage2 != null)
                {
                    instructionImage2.SetActive(false);
                    hasUserRightClickedDragged = true;
                }
            }
        }

        // Left Mouse Button (or specified dragButton) Down
        if (Input.GetMouseButtonDown(dragButton) && CanUse())
        {
            dragging = true;
            if (instructionImage != null)
            {
                instructionImage.SetActive(false);
            }
        }

        if (Input.GetMouseButton(dragButton) && dragging)
        {
            Vector3 mouseWorldPos = Vision.instance.GetMouseWorldPosition();
            if (mouseWorldPos.x <= leftmostX)
            {
                // Calculate target rotation based on drag
                Quaternion targetRotation = Quaternion.AngleAxis(CalculateLaunchVector().magnitude * -10f, Vector3.forward);
                // Smoothly interpolate to the target rotation
                hammer.localRotation = Quaternion.Lerp(hammer.localRotation, targetRotation, hammerRotationSpeed * Time.deltaTime);
                shadowHammer.localRotation = Quaternion.AngleAxis(previousLaunch.magnitude * -10f, Vector3.forward);

                if (trajectoryArrow != null)
                {
                    trajectoryArrow.SetActive(true); // Show the arrow during drag

                    // Position the arrow at the launch point
                    trajectoryArrow.transform.position = launchPoint.position;

                    // Calculate the trajectory vector
                    Vector3 trajectoryVector = CalculateLaunchVector() * launchPower;

                    // Rotate the arrow to match the trajectory direction
                    float angle = Mathf.Atan2(trajectoryVector.y, trajectoryVector.x) * Mathf.Rad2Deg;
                    trajectoryArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
            else
            {
                // Smoothly reset rotation to zero when the drag is invalid
                hammer.localRotation = Quaternion.Lerp(hammer.localRotation, Quaternion.identity, hammerRotationSpeed * Time.deltaTime);

                if (trajectoryArrow != null)
                {
                    trajectoryArrow.SetActive(false); // Hide the arrow when not dragging
                }
            }
        }
        else
        {
            if (trajectoryArrow != null)
            {
                trajectoryArrow.SetActive(false); // Hide the arrow when not dragging
            }

            // Check mouse release within the valid area
            if (Input.GetMouseButtonUp(dragButton) && dragging)
            {
                dragging = false;
                Vector3 mouseWorldPos = Vision.instance.GetMouseWorldPosition();
                if (mouseWorldPos.x <= leftmostX) // Launch only if released to the left of the leftmost point
                {
                    Vector3 launchForce = CalculateLaunchVector() * launchPower;
                    previousLaunch = CalculateLaunchVector();
                    Launch(launchForce, launchSpin, anvilPrefab);
                }
                hammer.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
        }
    }

}


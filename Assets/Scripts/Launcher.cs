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
    private bool  hasUserRightClickedDragged = false; // Flag 2 check if the user has dragged for first time
    [SerializeField] private float dragThreshold = 5.0f; // Threshold for drag detection
    private bool isRightClickDragging = false;
    private Vector3 initialRightClickPosition;



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

    }

    public void Launch(Vector3 launchForce, float angularForce, GameObject projectile)
    {
        if (projectile == null) { UnityEngine.Debug.Log("Null projectile");  return; }
        GameObject spawnedProjectile = Instantiate(projectile, launchPoint.position, Quaternion.identity);
        Rigidbody2D physicsBody = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (physicsBody == null) { Destroy(spawnedProjectile); UnityEngine.Debug.Log("Projectile missing rigidbody2D");return; }
        physicsBody.AddForce(launchForce, ForceMode2D.Impulse);
        physicsBody.angularVelocity = angularForce / physicsBody.mass;

        nextUsable = Time.time + useCooldown;
    }

    private Vector3 CalculateLaunchVector()
    {
        Vector3 offset = transform.position - Vision.instance.GetMouseWorldPosition();
        return offset.normalized * Mathf.Clamp(offset.magnitude / maxDrag,0f,1f) * maxDrag;
    }

    bool CanUse()
    {
        return triggerArea.OverlapPoint(Vision.instance.GetMouseWorldPosition()) && Time.time > nextUsable;
    }

    void Update()
    {
        anvilVisual.enabled = Time.time > nextUsable;
        shadowHammer.gameObject.SetActive(dragging);

        // Right Mouse Button Down
        if (Input.GetMouseButtonDown(1))
        {
            initialRightClickPosition = Input.mousePosition;
            isRightClickDragging = false;
        }

        // Right Mouse Button Held Down
        if (Input.GetMouseButton(1))
        {
            if (!isRightClickDragging && Vector3.Distance(initialRightClickPosition, Input.mousePosition) > dragThreshold) // Ensure you have defined dragThreshold
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

        // While Left Mouse Button (or specified dragButton) is Held Down
        else if (Input.GetMouseButton(dragButton) && dragging)
        {
            UnityEngine.Debug.DrawRay(transform.position, CalculateLaunchVector(), Color.yellow);
            hammer.localRotation = Quaternion.AngleAxis(CalculateLaunchVector().magnitude * -10f, Vector3.forward);
            shadowHammer.localRotation = Quaternion.AngleAxis(previousLaunch.magnitude * -10f, Vector3.forward);
        }

        // Left Mouse Button (or specified dragButton) Released
        else if (Input.GetMouseButtonUp(dragButton) && dragging)
        {
            dragging = false;
            Vector3 launchForce = CalculateLaunchVector() * launchPower;
            previousLaunch = CalculateLaunchVector();
            Launch(launchForce, launchSpin, anvilPrefab);
        }
    }

}


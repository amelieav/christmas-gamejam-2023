using System.Collections;
using System.Collections.Generic;
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


    private Vector3 previousLaunch;
    SpriteRenderer anvilVisual;
    Collider2D triggerArea;

    float nextUsable = 0f;
    bool dragging = false;

    void Start()
    {
        triggerArea = gameObject.GetComponent<Collider2D>();
        anvilVisual = GetComponent<SpriteRenderer>();
    }

    public void Launch(Vector3 launchForce, float angularForce, GameObject projectile)
    {
        if (projectile == null) { Debug.Log("Null projectile");  return; }
        GameObject spawnedProjectile = Instantiate(projectile, launchPoint.position, Quaternion.identity);
        Rigidbody2D physicsBody = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (physicsBody == null) { Destroy(spawnedProjectile);Debug.Log("Projectile missing rigidbody2D");return; }
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
        if (Input.GetMouseButtonDown(dragButton) && CanUse())
        {
            dragging = true;
        }
        else if (Input.GetMouseButton(dragButton) && dragging)
        {
            Debug.DrawRay(transform.position, CalculateLaunchVector(), Color.yellow);
            hammer.localRotation = Quaternion.AngleAxis(CalculateLaunchVector().magnitude*-10f, Vector3.forward);
            shadowHammer.localRotation = Quaternion.AngleAxis(previousLaunch.magnitude * -10f, Vector3.forward);
        }
        else if (Input.GetMouseButtonUp(dragButton) && dragging)
        {
            dragging = false;
            Vector3 launchForce = CalculateLaunchVector() * launchPower;
            previousLaunch = CalculateLaunchVector();
            Launch(launchForce, launchSpin, anvilPrefab);
        }
    }
}

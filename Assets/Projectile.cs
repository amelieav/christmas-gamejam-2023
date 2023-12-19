using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeToDisappear = 2f; // Time in seconds before the projectile is destroyed
    private float stationaryTimer = 0f; // Timer to track if the projectile hasn't moved
    private float groundContactTimer = 0f; // Timer to track contact with the ground
    private Vector3 lastPosition; // Last recorded position of the projectile
    private bool isContactingGround = false; // Flag to check if the projectile is contacting the ground

    void Start()
    {
        Vision.instance.SetTrack(transform);
        lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        Vision.instance.StopTracking();
    }

    void Update()
    {
        if (transform.position == lastPosition)
        {
            stationaryTimer += Time.deltaTime;
        }
        else
        {
            // Reset the stationary timer if the projectile has moved
            stationaryTimer = 0f;
        }

        lastPosition = transform.position;
        if (isContactingGround)
        {
            groundContactTimer += Time.deltaTime;
        }

        if (stationaryTimer >= timeToDisappear || groundContactTimer >= timeToDisappear)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isContactingGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isContactingGround = false;
            groundContactTimer = 0f;
        }
    }
}

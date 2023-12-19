using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class VisionBinder : MonoBehaviour
{
    Vision vision;
    BoxCollider2D trigger;

    void Start()
    {
        vision = GetComponentInChildren<Vision>();
        trigger = GetComponent<BoxCollider2D>();
        if (vision == null)
        {
            Debug.LogWarning("Failed to find a vision to bind");
            enabled = false;
        }
    }

    //late update to allow other script to move camera first before constrainting
    void LateUpdate()
    {
        Vector2 visionSize = vision.GetSize();
        Vector3 visionPosition = vision.transform.position;

        //limit camera vision to with the 2d area defined by the collider
        float minX = transform.position.x + trigger.offset.x - trigger.size.x / 2f + visionSize.x / 2f;
        visionPosition.x = Mathf.Max(minX, visionPosition.x);
        float maxX = transform.position.x + trigger.offset.x + trigger.size.x / 2f - visionSize.x / 2f;
        visionPosition.x = Mathf.Min(maxX, visionPosition.x);

        float minY = transform.position.y + trigger.offset.y - trigger.size.y / 2f + visionSize.y / 2f;
        visionPosition.y = Mathf.Max(minY, visionPosition.y);
        float maxY = transform.position.y + trigger.offset.y + trigger.size.y / 2f - visionSize.y / 2f;
        visionPosition.y = Mathf.Min(maxY, visionPosition.y);

        //apply changes
        vision.transform.position = visionPosition;
    }
}

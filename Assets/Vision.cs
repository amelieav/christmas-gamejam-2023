using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] int panButton;

    Transform trackedObject = null;
    new Camera camera;

    Vector3 panMouseAnchor = Vector3.zero;
    Vector3 panWorldAnchor = Vector3.zero;
    float screenToWorldScale = 1f;

    private static Vision instance;

    private void Awake()
    {
        instance = this; 
    }

    public static void SetTrack(Transform transform)
    {
        instance.trackedObject = transform;
    }

    public static void StopTracking()
    {
        instance.trackedObject = null;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        return instance.camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        screenToWorldScale = (camera.ScreenToWorldPoint(Vector3.right) - camera.ScreenToWorldPoint(Vector3.zero)).magnitude;
    }

    private void Track()
    {
        if (trackedObject == null) { return; }

    }

    private void Pan()
    {
        if (trackedObject != null) { return;}
        if (Input.GetMouseButtonDown(panButton))
        {
            panWorldAnchor = transform.position;
            panMouseAnchor = Input.mousePosition;
        }
        else if (Input.GetMouseButton(panButton))
        {
            transform.position = (panMouseAnchor - Input.mousePosition) * screenToWorldScale + panWorldAnchor;
        }
        else if(Input.GetMouseButtonUp(panButton))
        {
            panWorldAnchor = Vector3.zero;
            panMouseAnchor = Vector3.zero;
        }
    }

    void Update()
    {
        Track();
        Pan();
    }
}

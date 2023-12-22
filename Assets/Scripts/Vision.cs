using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] int panButton;

    [Header("Settings")]
    [SerializeField] float trackSpeed;
    [SerializeField] Transform home;

    public Transform trackedObject { get; private set; }
    new Camera camera;

    Vector3 panMouseAnchor = Vector3.zero;
    Vector3 panWorldAnchor = Vector3.zero;
    float screenToWorldScale = 1f;

    public static Vision instance { get; private set; }

    private void Awake()
    {
        instance = this; 
    }

    /// <summary>
    /// Set the camera to track an object
    /// </summary>
    /// <param name="transform"></param>
    public void SetTrack(Transform transform)
    {
        trackedObject = transform;
    }

    /// <summary>
    /// Reset the tracking status of the camera
    /// </summary>
    public void StopTracking()
    {
        trackedObject = null;
    }

    public void GoHome()
    {
        SetTrack(home);
    }

    /// <summary>
    /// Calculates the world position of the cursor
    /// </summary>
    /// <returns>x, y</returns>
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        return position;
    }

    /// <summary>
    /// Calculates the size of the camera view in world units
    /// </summary>
    /// <returns>width, height</returns>
    public Vector2 GetSize()
    {
        return new Vector2(instance.camera.orthographicSize * 2 * instance.camera.aspect, instance.camera.orthographicSize * 2);
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        screenToWorldScale = (camera.ScreenToWorldPoint(Vector3.right) - camera.ScreenToWorldPoint(Vector3.zero)).magnitude;

        GoHome();
    }

    /// <summary>
    /// Moves the camera to follow another object
    /// </summary>
    private void Track()
    {
        float z = transform.position.z;
        Vector2 position = transform.position;
        Vector2 target = trackedObject.transform.position;
        position = Vector2.Lerp(position, target, Time.deltaTime * trackSpeed);
        transform.position = new Vector3(position.x, position.y, z);
    }

    /// <summary>
    /// Pan the camera in response to player holding down a mouse button and moving the cursor
    /// </summary>
    private void Pan()
    {
        
        //save an anchor when starts panning and sets the position based on cursor displacement relative to anchor
        if (Input.GetMouseButtonDown(panButton))
        {
            panWorldAnchor = transform.position;
            panMouseAnchor = Input.mousePosition;

            if (trackedObject)
            {
                trackedObject = null;
            }
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
        Pan();
        if (trackedObject != null)
        {
            Track();
        }
    }
}

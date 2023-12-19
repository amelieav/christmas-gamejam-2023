using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float multiplier;

    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 position = transform.position;

        position.x = Vision.instance.transform.position.x * multiplier * Mathf.Sign(position.z);

        transform.position = position;
    }
}

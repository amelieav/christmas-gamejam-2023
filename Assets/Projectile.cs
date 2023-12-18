using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.position.y < -100f)
        {
            Destroy(gameObject);
        }
    }
}

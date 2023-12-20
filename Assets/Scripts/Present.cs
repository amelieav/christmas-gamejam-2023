using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] string projectileTag;

    private bool isCollected = false;

    public GameObject CollectedPresentTextHolder; // Ensure this variable name matches your GameObject

    // the following function flashes up a message when the Player collects the present
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag(projectileTag))
        {
            isCollected = true;

            // Access the TextMesh component within the CollectedPresentTextHolder
            TextMesh textMesh = CollectedPresentTextHolder.GetComponent<TextMesh>();

            // Check if the TextMesh component exists before enabling/disabling it
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true); // Enable the TextMesh GameObject
            }

            gameObject.SetActive(false);
        }
    }
}

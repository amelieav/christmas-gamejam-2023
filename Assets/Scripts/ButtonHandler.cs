using System.Diagnostics;
using UnityEngine;

public class DetectClicks : MonoBehaviour
{
    public string tagToHide = "Scene1";

    public void Pressed()
    {
        // Find the GameObject with the specified tag
        GameObject objectToHide = GameObject.FindGameObjectWithTag(tagToHide);

        // If the object is found, deactivate it
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
            UnityEngine.Debug.Log("Clicked and hid the object with tag: " + tagToHide);
        }
        else
        {
            UnityEngine.Debug.Log("No object with the tag " + tagToHide + " found!");
        }
    }
}

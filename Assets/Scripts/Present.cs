using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] string projectileTag;

    private bool isCollected = false;

    public GameObject CollectedPresentTextHolder; // Ensure this variable name matches your GameObject

    // This function is called when a collision with a 2D collider occurs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollected && collision.collider.CompareTag(projectileTag))
        {
            isCollected = true;

            // Call the static IncrementScore method on ScoreManager
            ScoreManager.IncrementScore(5);

            // Access the TextMesh component within the CollectedPresentTextHolder
            TextMesh textMesh = CollectedPresentTextHolder.GetComponent<TextMesh>();

            // Check if the TextMesh component exists before enabling/disabling it
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true); // Enable the TextMesh GameObject
            }

            gameObject.SetActive(false); // Disable the present GameObject
        }
    }
}

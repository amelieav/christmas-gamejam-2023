using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] string projectileTag;
    private bool isCollected = false;
    public GameObject CollectedPresentTextHolder;
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollected && collision.collider.CompareTag(projectileTag))
        {
            isCollected = true;

            // Call the static IncrementScore method on ScoreManager
            ScoreManager.IncrementScore(5);

            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Move the present out of camera's view so the audio clip keeps playing
            // (if u just deactivate the present then it stops the audio)
            transform.position = new Vector3(transform.position.x, 3000, transform.position.z);

            if (audioSource != null && audioSource.clip != null)
            {
                Invoke("DeactivateGameObject", audioSource.clip.length);
            }
        }
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}

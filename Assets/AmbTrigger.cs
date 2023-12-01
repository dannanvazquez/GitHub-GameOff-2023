using UnityEngine;

public class AmbTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enterSFX;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider belongs to the player and has not been triggered yet
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            // Play the enterSFX
            if (audioSource != null && enterSFX != null)
            {
                audioSource.clip = enterSFX;
                audioSource.Play();
                hasBeenTriggered = true; // Set the flag to true after playing the audio
            }
        }
    }
}
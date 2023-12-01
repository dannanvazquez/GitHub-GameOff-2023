using UnityEngine;

public class RoomMusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlayerInside = false;

    // Reference to the boss GameObject
    public GameObject bossObject;
    public GameObject healthUI;

    public GameObject activateObject;
    public GameObject deactivateObject1;
    public GameObject deactivateObject2;
        public GameObject deactivateObject3;
    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // Make sure there's an AudioSource component
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found. Please attach an AudioSource component to this GameObject.");
        }

        // Ensure the bossObject is set in the Unity Editor
        if (bossObject == null)
        {
            Debug.LogError("Boss GameObject not set. Please assign the boss GameObject in the Unity Editor.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the room
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInside)
            {
                // Play the room entrance music from the beginning
                PlayRoomMusic();
                healthUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the room
        if (other.CompareTag("Player"))
        {
            // Stop the music if the player exits the room
            FadeOutRoomMusic();
            healthUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the bossObject is destroyed
        if (bossObject == null)
        {
            // Stop the music if the boss is destroyed
            FadeOutRoomMusic();
            ActivateDeactivateObjects();
            healthUI.SetActive(false);
        }
    }

    void PlayRoomMusic()
    {
        // Check if the AudioSource component and AudioClip are set
        if (audioSource != null && audioSource.clip != null)
        {
            // Play the audio clip from the beginning
            audioSource.Play();
            isPlayerInside = true;
        }
        else
        {
            Debug.LogError("AudioSource component or AudioClip not set. Please set them in the Unity Editor.");
        }
    }

    void FadeOutRoomMusic(float fadeDuration = 2f)
    {
        // Check if the AudioSource component is set
        if (audioSource != null)
        {
            // StartCoroutine is used to start a coroutine method
            StartCoroutine(FadeOutCoroutine(fadeDuration));
            isPlayerInside = false;

        }
    }

    System.Collections.IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        // Gradually reduce the volume over the specified duration
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t);
            yield return null;
        }

        // Ensure the volume is set to 0
        audioSource.volume = 0;

        // Stop the music
        audioSource.Stop();
    }

void ActivateDeactivateObjects()
{
    Debug.Log("ActivateDeactivateObjects method called.");
    deactivateObject3.SetActive(false);
    // Activate the specified game object
    if (activateObject != null)
    {
        Debug.Log("Activating: " + activateObject.name);
        activateObject.SetActive(true);
    }
    else
    {
        Debug.LogError("Activate GameObject not set. Please assign the Activate GameObject in the Unity Editor.");
    }

    // Deactivate the specified game objects
    if (deactivateObject1 != null)
    {
        Debug.Log("Deactivating: " + deactivateObject1.name);
        deactivateObject1.SetActive(false);
    }
    else
    {
        Debug.LogError("DeactivateObject1 not set. Please assign DeactivateObject1 in the Unity Editor.");
    }

    if (deactivateObject2 != null)
    {
        Debug.Log("Deactivating: " + deactivateObject2.name);
        deactivateObject2.SetActive(false);
    }
    else
    {
        Debug.LogError("DeactivateObject2 not set. Please assign DeactivateObject2 in the Unity Editor.");
    }
}
}

using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject VFXActive, VFXInactive, VFXActivationExplosion;
    [SerializeField] private AudioSource savepoint_audiosource;
    [SerializeField] private AudioClip save_sfx;

    private void Start()
    {
        VFXActive.SetActive(false);
        VFXInactive.SetActive(true);
        VFXActivationExplosion.SetActive(false);

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player is pressing the "E" key
            if (Input.GetKeyDown(KeyCode.E))
            {
                VFXActivationExplosion.SetActive(true);
                VFXActive.SetActive(true);
                VFXInactive.SetActive(false);
                savepoint_audiosource.Play();

                GameManager.Instance.SetRespawnPoint(transform.position);
                Debug.Log("Respawn point set!");
            }
        }
    }
}
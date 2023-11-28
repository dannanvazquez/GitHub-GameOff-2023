using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject VFXActive, VFXInactive, VFXActivationExplosion;

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


                GameManager.Instance.SetRespawnPoint(transform.position);
                Debug.Log("Respawn point set!");
            }
        }
    }
}
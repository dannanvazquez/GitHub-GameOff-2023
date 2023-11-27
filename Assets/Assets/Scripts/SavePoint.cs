using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player is pressing the "E" key
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.SetRespawnPoint(transform.position);
                Debug.Log("Respawn point set!");
            }
        }
    }
}
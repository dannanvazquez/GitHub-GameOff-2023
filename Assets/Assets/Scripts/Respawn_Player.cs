using UnityEngine;

public class Respawn_Player : MonoBehaviour
{
    void Update()
    {
        // You can trigger respawn manually, for example, by pressing a key
        if (Input.GetKeyDown(KeyCode.E))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        GameManager.Instance.RespawnPlayer(gameObject);
    }
}
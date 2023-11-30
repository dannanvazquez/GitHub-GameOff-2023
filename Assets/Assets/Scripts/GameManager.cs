using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 respawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }

    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = respawnPoint;
        // Reset other player attributes here if needed

        // Reset enemies
        ResetEnemies();
    }
    
private void ResetEnemies()
{
    // Get all enemy GameObjects in the scene with the "BossEnemy" tag
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("BossEnemy");
    foreach (GameObject enemy in enemies)
    {
        // Print the current position before the reset
        Debug.Log("Boss Current Position: " + enemy.transform.position);

        // Reset boss position
        //enemy.transform.position = new Vector3(-3972f, 0.48f, 1433.9f);

        // Print the position after the reset
        Debug.Log("Boss New Position: " + enemy.transform.position);
    }
}


}

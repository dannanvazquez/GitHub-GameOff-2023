using UnityEngine;
using System.Collections.Generic;

public class Medkit : MonoBehaviour
{
    public GameObject playerPrefab;
    public float healthToAdd = 50f;  // Amount of health to add on pickup
    
    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab; 
    public GameObject uiLootingPrefab; 

    // List to keep track of active UI popups
    private List<GameObject> activeUIPopups = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsPlayerInTrigger())
            {
                PickUp();
            }
        }
    }

    private bool IsPlayerInTrigger()
    {
        return true;
    }

    private void PickUp()
    {
        DestroyExistingUIPopups();

        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        // Modify player health
        PlayerHealth playerHealth = playerPrefab.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.currentHealth += healthToAdd;
            Debug.Log("Medkit picked up. Health to add: " + healthToAdd);
            if (playerHealth.currentHealth > playerHealth.maxHealth)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
            }

            playerHealth.UpdateHealthBar();
        }

        Destroy(gameObject);
        audiosource_pickup.Play();

        Canvas canvasInstance = Instantiate(canvasPrefab);
        GameObject uiLootingInstance = Instantiate(uiLootingPrefab);
        uiLootingInstance.transform.SetParent(canvasInstance.transform, false);
        canvasInstance.gameObject.SetActive(true);
        uiLootingInstance.SetActive(true);
        activeUIPopups.Add(uiLootingInstance);
    }

    private void DestroyExistingUIPopups()
    {
        foreach (GameObject popup in activeUIPopups)
        {
            Destroy(popup);
        }

        activeUIPopups.Clear();
    }
}

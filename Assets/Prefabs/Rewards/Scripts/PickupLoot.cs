using UnityEngine;
using System.Collections.Generic;

public class PickupObject: MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerCombat playerCombat; // To change the stat of the playerCombat (!!WE MUST RESET AFTER EACH USE!!)
    
    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab; 
    public GameObject uiLootingPrefab; 

    // List to keep track of active UI popups
    private List<GameObject> activeUIPopups = new List<GameObject>();

    void Start()
    {
        playerCombat = playerPrefab.GetComponent<PlayerCombat>();
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
        // Destroy any existing UI popups
        DestroyExistingUIPopups();

        // Instantiate particles at the position of the pickup object
        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        playerCombat.meleeDamage += 5; // Stats change
        Debug.Log("New MeleeDamage: " + playerCombat.meleeDamage);

        audiosource_pickup.Play();

        // Instantiate the Canvas
        Canvas canvasInstance = Instantiate(canvasPrefab);

        // Instantiate the UI_Looting prefab
        GameObject uiLootingInstance = Instantiate(uiLootingPrefab);

        // Make the UI_Looting prefab a child of the Canvas
        uiLootingInstance.transform.SetParent(canvasInstance.transform, false);

        // Optionally, you can set the position and other properties of the UI_Looting prefab within the Canvas
        // uiLootingInstance.transform.localPosition = new Vector3(x, y, 0);
        
        // Enable the Canvas (if it's not enabled by default)
        canvasInstance.gameObject.SetActive(true);

        // Activate the UI_Looting prefab
        uiLootingInstance.SetActive(true);

        // Add the new UI popup to the list of active UI popups
        activeUIPopups.Add(uiLootingInstance);
    }

    // Destroy any existing UI popups
    private void DestroyExistingUIPopups()
    {
        foreach (GameObject popup in activeUIPopups)
        {
            Destroy(popup);
        }

        // Clear the list of active UI popups
        activeUIPopups.Clear();
    }
}

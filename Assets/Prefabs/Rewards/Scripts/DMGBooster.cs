using UnityEngine;
using System.Collections.Generic;

public class PickupObjectDMGBooster : MonoBehaviour
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
        DestroyExistingUIPopups();

        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        playerCombat.meleeDamage += 2; // Stats change
        Debug.Log("New MeleeDamage: " + playerCombat.meleeDamage);

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

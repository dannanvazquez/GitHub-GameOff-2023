using UnityEngine;
using System.Collections.Generic;

public class PickupObjectArrows : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerInventoryManager playerInventoryManager;
    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab; 
    public GameObject uiLootingPrefab; 

    private List<GameObject> activeUIPopups = new List<GameObject>();

    void Start()
    {
        playerInventoryManager = playerPrefab.GetComponent<PlayerInventoryManager>();
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
    // Destroy loot, create particles
    Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

    if (playerInventoryManager.ammoCounts.Count == 0)
    {
        playerInventoryManager.ammoCounts.Add(0);
    }

    // Index : 0:fire 1:explo 2:ice 3:random
    playerInventoryManager.ammoCounts[1] += 1;

    // Play sound on pickUp
    audiosource_pickup.Play();
    Debug.Log("Ammo Counts After Pickup: " + string.Join(", ", playerInventoryManager.ammoCounts));

    Destroy(gameObject, 0.1f);

    // Canvas writing
    Canvas canvasInstance = Instantiate(canvasPrefab);
    GameObject uiLootingInstance = Instantiate(uiLootingPrefab);
    uiLootingInstance.transform.SetParent(canvasInstance.transform, false);
    canvasInstance.gameObject.SetActive(true);
    uiLootingInstance.SetActive(true);
    activeUIPopups.Add(uiLootingInstance);
}
}

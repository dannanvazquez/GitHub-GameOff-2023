using System.Collections.Generic;
using UnityEngine;

public class PickupObjectArrows : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The type of arrow that will be added.")]
    [SerializeField] private ArrowTypes arrowType;
    [Tooltip("The amount of ammo to add.")]
    [SerializeField] private int addAmmoAmount;

    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab;
    public GameObject uiLootingPrefab;

    private List<GameObject> activeUIPopups = new List<GameObject>();

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (IsPlayerInTrigger()) {
                PickUp();
            }
        }
    }

    private bool IsPlayerInTrigger() {
        return true;
    }

    private void PickUp() {
        // TODO: Display to the player that their ammo is full and can't pickup.
        if (PlayerInventoryManager.Instance.IsFullAmmo(arrowType)) return;
    
        // Destroy loot, create particles
        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        PlayerInventoryManager.Instance.AddAmmo(arrowType, addAmmoAmount);

        // Play sound on pickUp
        audiosource_pickup.Play();
        //Debug.Log("Ammo Counts After Pickup: " + string.Join(", ", PlayerInventoryManager.Instance.ammoCounts));

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

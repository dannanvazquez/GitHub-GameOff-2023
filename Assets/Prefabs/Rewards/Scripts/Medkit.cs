using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour {
    public float healthToAdd = 50f;  // Amount of health to add on pickup

    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab;
    public GameObject uiLootingPrefab;

    // List to keep track of active UI popups
    private List<GameObject> activeUIPopups = new List<GameObject>();

    private bool isPlayerInTrigger;

    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger) {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) isPlayerInTrigger = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) isPlayerInTrigger = false;
    }

    private void PickUp() {
        if (!PlayerInventoryManager.Instance.TryGetComponent(out PlayerHealth playerHealth)) return;

        if (playerHealth.currentHealth >= playerHealth.maxHealth) {
            // TODO: Notify the player that their health is full and can't heal.
            return;
        }

        DestroyExistingUIPopups();

        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        // Modify player health
        playerHealth.Heal(healthToAdd);

        Destroy(gameObject);
        audiosource_pickup.Play();

        Canvas canvasInstance = Instantiate(canvasPrefab);
        GameObject uiLootingInstance = Instantiate(uiLootingPrefab);
        uiLootingInstance.transform.SetParent(canvasInstance.transform, false);
        canvasInstance.gameObject.SetActive(true);
        uiLootingInstance.SetActive(true);
        activeUIPopups.Add(uiLootingInstance);
    }

    private void DestroyExistingUIPopups() {
        foreach (GameObject popup in activeUIPopups) {
            Destroy(popup);
        }

        activeUIPopups.Clear();
    }
}

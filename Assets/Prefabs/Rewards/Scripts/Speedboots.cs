using UnityEngine;
using System.Collections.Generic;

public class Speedboots : MonoBehaviour
{
    public AudioSource audiosource_pickup;
    public GameObject pickupParticlesPrefab;
    public Canvas canvasPrefab; 
    public GameObject uiLootingPrefab; 

    public float speedBoostAmount = 2f;
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

    private void PickUp()
    {
        DestroyExistingUIPopups();
        PlayerMovement playerMovement = PlayerInventoryManager.Instance.GetComponent<PlayerMovement>();
        Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);
        

        if (playerMovement != null)
        {
            playerMovement.walkSpeed += speedBoostAmount;
            playerMovement.runSpeed += speedBoostAmount;
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

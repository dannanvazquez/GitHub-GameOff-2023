using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootManager : MonoBehaviour
{
    public GameObject firstLootPrefab;
    public GameObject secondLootPrefab;
    public GameObject thirdLootPrefab;
    public GameObject fourthLootPrefab;
    public GameObject fifthLootPrefab;
    public GameObject sixthLootPrefab;
    public GameObject seventhLootPrefab;

    private GameObject currentLoot;
    private Vector3 originalScale;

    public TextMeshProUGUI lootNameText; // Reference to the TextMeshProUGUI component
    public Canvas lootCanvas; // Reference to the Canvas component

    public void InstantiateRandomLoot(Vector3 chestPosition)
    {
        // Check if there's an existing loot, and if not, instantiate a new one
        if (currentLoot == null)
        {
            int randomIndex = Random.Range(0, 7); // 0-6: firstLoot, 1-6: secondLoot, 2-6: thirdLoot, 3-6: fourthLoot, 4-6: fifthLoot, 5-6: sixthLoot, 6: seventhLoot

            GameObject lootPrefab = null;
            string lootName = "";

            switch (randomIndex)
            {
                case 0:
                    lootPrefab = firstLootPrefab;
                    lootName = "First Loot";
                    break;
                case 1:
                    lootPrefab = secondLootPrefab;
                    lootName = "Second Loot";
                    break;
                case 2:
                    lootPrefab = thirdLootPrefab;
                    lootName = "Third Loot";
                    break;
                case 3:
                    lootPrefab = fourthLootPrefab;
                    lootName = "Fourth Loot";
                    break;
                case 4:
                    lootPrefab = fifthLootPrefab;
                    lootName = "Fifth Loot";
                    break;
                case 5:
                    lootPrefab = sixthLootPrefab;
                    lootName = "Sixth Loot";
                    break;
                case 6:
                    lootPrefab = seventhLootPrefab;
                    lootName = "Seventh Loot";
                    break;
            }

            if (lootPrefab != null)
            {
                Vector3 adjustedPosition = chestPosition + new Vector3(0f, 1.5f, -0.28f);

                // Instantiate new loot
                currentLoot = Instantiate(lootPrefab, adjustedPosition, Quaternion.identity);

                // Update the TextMeshProUGUI component with the loot name
                if (lootNameText != null)
                {
                    lootNameText.text = lootName;
                }

                // Save the original scale
                originalScale = currentLoot.transform.localScale;

                // Show the canvas
                if (lootCanvas != null)
                {
                    lootCanvas.enabled = true;
                }

                // Adjust the scale of the instantiated loot
                StartCoroutine(ScaleOverTime(currentLoot.transform, new Vector3(0f, 0f, 0f), originalScale, 1.0f));
            }
        }
    }

    public void DestroyLoot()
    {
        // Check if there's an existing loot, and if so, destroy it
        if (currentLoot != null)
        {
            StartCoroutine(DestroyLootWithDelay());
        }
    }

    IEnumerator DestroyLootWithDelay()
    {
        // Add a delay before scaling down and destroying the loot
        yield return new WaitForSeconds(.5f);

        // Scale down the loot before destroying
        StartCoroutine(ScaleOverTime(currentLoot.transform, currentLoot.transform.localScale, new Vector3(0f, 0f, 0f), 1.0f));

        // Add a delay before destroying to allow scaling animation
        yield return new WaitForSeconds(1.0f);

        // Hide the canvas
        if (lootCanvas != null)
        {
            lootCanvas.enabled = false;
        }

        // Destroy the loot
        Destroy(currentLoot);

        // Set currentLoot to null after destruction
        currentLoot = null;
    }

    IEnumerator ScaleOverTime(Transform objectToScale, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            objectToScale.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // Ensure the final scale is exactly as intended
        objectToScale.localScale = toScale;
    }
}

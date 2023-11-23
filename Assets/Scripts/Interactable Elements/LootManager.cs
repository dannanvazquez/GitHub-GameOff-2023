using System.Collections;
using UnityEngine;

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

    public void InstantiateRandomLoot(Vector3 chestPosition)
    {
        // Check if there's an existing loot, and if not, instantiate a new one
        if (currentLoot == null)
        {
            int randomIndex = Random.Range(0, 7);
            GameObject lootPrefab = GetLootPrefab(randomIndex);

            if (lootPrefab != null)
            {
                Vector3 adjustedPosition = chestPosition + new Vector3(0f, 1.5f, -0.28f);

                // Instantiate new loot
                currentLoot = Instantiate(lootPrefab, adjustedPosition, Quaternion.identity);

                // Save the original scale
                originalScale = currentLoot.transform.localScale;

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

        // Destroy the loot
        Destroy(currentLoot);

        // Set currentLoot and currentLootParticles to null after destruction
        currentLoot = null;

    }

    private GameObject GetLootPrefab(int index)
    {
        switch (index)
        {
            case 0:
                return firstLootPrefab;
            case 1:
                return secondLootPrefab;
            case 2:
                return thirdLootPrefab;
            case 3:
                return fourthLootPrefab;
            case 4:
                return fifthLootPrefab;
            case 5:
                return sixthLootPrefab;
            case 6:
                return seventhLootPrefab;
            default:
                return null;
        }
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

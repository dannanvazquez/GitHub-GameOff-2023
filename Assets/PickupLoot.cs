using UnityEngine;
using TMPro;

public class PickupObject : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerCombat playerCombat; // To change the stat of the playerCombat (!!WE MUST RESET AFTER EACH USE!!)

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
        Destroy(gameObject);
        playerCombat.meleeDamage += 5; // Stats change
        Debug.Log("New MeleeDamage: " + playerCombat.meleeDamage);
    }
}

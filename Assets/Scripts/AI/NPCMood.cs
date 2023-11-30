using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMood : MonoBehaviour
{
    public bool isGameEnded=false;
    public Animator[] animatorNPC;
    public GameObject areaBlockerBossLOCKPAD;
    // Start is called before the first frame update
    void Start()
    {
        areaBlockerBossLOCKPAD.SetActive(false);

        if (isGameEnded)
        {
            for (int i = 0; i < animatorNPC.Length; i++)
            {
                animatorNPC[i].SetBool("Celebrate", true);
            }
        }
        
        

    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            areaBlockerBossLOCKPAD.SetActive(true);
            NPCsInPrison();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deactivate the lock when the player exits the room
            areaBlockerBossLOCKPAD.SetActive(false);
        }
    }

    public void NPCsInPrison()
    {
        for (int i = 0; i < animatorNPC.Length; i++)
        {
            animatorNPC[i].SetBool("inPrison", true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMood : MonoBehaviour
{
    public bool isGameEnded=false;
    public Animator[] animatorNPC; 
    // Start is called before the first frame update
    void Start()
    {
        if (isGameEnded)
        {
            for (int i = 0; i < animatorNPC.Length; i++)
            {
                animatorNPC[i].SetBool("Celebrate", true);
            }
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NPCsInPrison()
    {
        for (int i = 0; i < animatorNPC.Length; i++)
        {
            //animatorNPC[i].SetBool("inPrison", true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableElement : MonoBehaviour
{
    public GameObject BoxPartsWRigidbody, BoxPartsNoRB;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
          

            BoxPartsNoRB.SetActive(false);
            BoxPartsWRigidbody.SetActive(true);
            
            Destroy(this.gameObject, 4f);
        }
    }
}

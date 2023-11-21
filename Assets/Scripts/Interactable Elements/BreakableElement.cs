using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableElement : MonoBehaviour
{
    public GameObject mygameObject;
    public Collider collider_arrow;

    private void OnCollisionEnter(Collision collision)
    { 
        Debug.Log("Collision detected!");

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Arrow hit!");
            Destroy(mygameObject);
        }
    }
}

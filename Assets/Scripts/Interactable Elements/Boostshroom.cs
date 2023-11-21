using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostshroom : MonoBehaviour
{
    [Tooltip("The amount of force to apply to the colliding rigidbody.")]
    public float yForce = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player") return;
        if (collision.body == null) return;

        Rigidbody rb = (Rigidbody)collision.body;
        rb.AddForce(new Vector3(0, yForce * 1000, 0));
    }
}

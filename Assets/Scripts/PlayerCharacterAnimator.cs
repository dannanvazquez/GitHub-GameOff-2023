using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimator : MonoBehaviour
{
    private Animator animator;
    public float speedFactor = 8.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        float speed = moveDirection.magnitude*speedFactor;

        // Move the character
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);

        // Update the Animator parameters
        animator.SetFloat("speed", speed);
    }
}




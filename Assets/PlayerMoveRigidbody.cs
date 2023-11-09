using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRigidbody : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rb;
    private Animator animator;
    private bool jumpInput = false;

    private float speed = 300f;
    private float rotationSpeed = 200f;
    private float quickRotationSpeed = 300f;

    private float jumpForce = 200f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // this gives us a number betweeen -1 and 1
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Debug.Log("Hor = " + horizontalInput.ToString());
        Debug.Log("Vert = " + verticalInput.ToString());

        float speed = horizontalInput * 10f + verticalInput * 10f;

        // Update the Animator parameters for the Blend Tree
        animator.SetFloat("speed", speed);
        Debug.Log("speed = " + speed.ToString());

        // Check for jump input (space key or mouse button)
        jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (jumpInput)
        {
            animator.SetBool("IsJumping", true);

            // Reset the "IsJumping" parameter after the jump animation completes
            Invoke("ResetJumpParameter", 1.0f);
        }
    }

    // Physics Update 
    void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
    }

    void MovePlayer()
    {
        rb.velocity = (transform.forward * verticalInput) * speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontalInput) * rotationSpeed * Time.fixedDeltaTime);

        if (verticalInput == -1)
        {
            transform.Rotate((transform.up * verticalInput) * quickRotationSpeed * Time.fixedDeltaTime);
        }

    }

    void JumpPlayer()
    {
        // If jump input is detected, trigger the jump animation
        if (jumpInput)
        {
            // Perform other jump-related actions (e.g., apply force, play sound, etc.)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }
    }

    void ResetJumpParameter()
    {
        // Reset the "IsJumping" parameter in the Animator
        animator.SetBool("IsJumping", false);
    }
}

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Animator animator;

    private CharacterController characterController;
private bool isJumping = false;
    [Header("Settings")]
    [Tooltip("How fast the player walks.")]
    public float walkingSpeed = 7.5f;
    [Tooltip("How fast the player runs.")]
    public float runningSpeed = 11.5f;
    [Tooltip("The velocity speed upwards when the player jumps.")]
    public float jumpSpeed = 8.0f;

    private bool canMove = true;

    private Vector2 receivedInput;
    private Vector3 moveDirection;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

private void Update() {
    // Retrieve player inputs
    receivedInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    if (characterController.isGrounded) {
        if (Input.GetButtonDown("Jump") && canMove && Time.timeScale != 0 && !isJumping) {
            isJumping = true;
            animator.SetTrigger("Jump");
            moveDirection.y = jumpSpeed;
        } else {
            moveDirection.y = 0;
        }
    } else {
        isJumping = false; // Reset the jump flag when the player is not grounded
    }

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = (canMove && Time.timeScale != 0) ? receivedInput.y : 0;
        float curSpeedY = (canMove && Time.timeScale != 0) ? receivedInput.x : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.Normalize();
        moveDirection.x *= (isRunning ? runningSpeed : walkingSpeed);
        moveDirection.z *= (isRunning ? runningSpeed : walkingSpeed);

        moveDirection.y = movementDirectionY;

        animator.SetFloat("Velocity", characterController.velocity.x*characterController.velocity.x + characterController.velocity.z*characterController.velocity.z);
        animator.SetBool("IsGrounded", characterController.isGrounded);
        animator.SetBool("IsFalling", characterController.velocity.y < 0.01f);
        animator.SetBool("IsRunning", isRunning);
    }

    private void FixedUpdate() {
        if (!characterController.isGrounded) {
            moveDirection += Physics.gravity * Time.fixedDeltaTime;
        }

        if (characterController.enabled) characterController.Move(moveDirection * Time.fixedDeltaTime);
    }
}

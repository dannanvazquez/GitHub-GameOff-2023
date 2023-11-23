using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;
    [SerializeField] private Image staminaBar;
    [SerializeField] private ParticleSystem sleepParticles;

    private Rigidbody rb;
    private PlayerStamina playerStamina; // Reference to PlayerStamina script
    private PlayerCombat playerCombat;

    [Header("Settings")]
    [Tooltip("The speed at which you move when walking.")]
    [SerializeField] private float walkSpeed;
    [Tooltip("The speed at which you move when running.")]
    [SerializeField] private float runSpeed;
    [Tooltip("The drag your rigidbody has when on the ground.")]
    [SerializeField] private float groundDrag;
    [Tooltip("The force that you gain when jumping.")]
    [SerializeField] private float jumpForce;
    [Tooltip("The amount of seconds before you can jump again.")]
    [SerializeField] private float jumpCooldown;
    [Tooltip("The speed at which your movement is multiplied while in the air.")]
    [SerializeField] private float airMultiplier;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float moveSpeed;

    private bool isGrounded;
    private bool isRunning;
    private bool canJump = true;
    private bool isAsleep;

    private float stamina;

    private Coroutine recharge;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStamina = GetComponent<PlayerStamina>(); // Assuming PlayerStamina script is on the same GameObject as PlayerMovement
        playerCombat = GetComponent<PlayerCombat>();
        UpdateStaminaBar();
    }


    private void Update() {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        MyInput();
        SpeedControl();
        AnimationParameters();

        // Handle drag
        if (isGrounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        if (isAsleep) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetButton("Run") && playerStamina.UseStamina(playerStamina.RunCost * Time.deltaTime);
        moveSpeed = isRunning ? runSpeed : walkSpeed;

        // Check for jumping
        if (Input.GetButton("Jump") && canJump && isGrounded) {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
                // Manage stamina recharge if not running
        if (!isRunning && stamina < playerStamina.MaxStamina)
        {
            playerStamina.RechargeStamina();
        }
    }

    private void MovePlayer() {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Movement on ground
        if (isGrounded) {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        } else {
            // Movement in air
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
    }

    private void SpeedControl() {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVelocity.magnitude > moveSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        animator.SetTrigger("Jump");
    }

    private void ResetJump() {
        canJump = true;
    }

    private void AnimationParameters() {
        animator.SetFloat("Velocity", rb.velocity.x*rb.velocity.x + rb.velocity.z*rb.velocity.z);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsFalling", rb.velocity.y < 0.01f);
        animator.SetBool("IsRunning", isRunning);
    }

    private IEnumerator RechargeStamina()
    {
        while (stamina < playerStamina.MaxStamina)
        {
            stamina += playerStamina.ChargeRate / 50f;
            stamina = Mathf.Min(playerStamina.MaxStamina, stamina);
            UpdateStaminaBar();
            yield return new WaitForSeconds(0.05f);
        }
        recharge = null;
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / playerStamina.MaxStamina;
        }
    }

    public void Sleep(float sleepTime) {
        StartCoroutine(SleepCoroutine(sleepTime));
    }

    // TODO: Seperate script to use as a reference for isAsleep? Since many scripts depend on it.
    private IEnumerator SleepCoroutine(float sleepTime) {
        isAsleep = true;
        playerCombat.isAsleep = true;
        playerCombat.playerCamera.isAsleep = true;
        sleepParticles.Play();
        horizontalInput = 0;
        verticalInput = 0;

        yield return new WaitForSeconds(sleepTime);

        isAsleep = false;
        playerCombat.isAsleep = false;
        playerCombat.playerCamera.isAsleep = false;
        sleepParticles.Stop();
    }
}
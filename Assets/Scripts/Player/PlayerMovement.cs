using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObject;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem sleepParticles;

    // Other script references
    private Rigidbody rb;
    private PlayerStamina playerStamina;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;

    // Movement settings
    [Header("Settings")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float rollCooldown;
    [SerializeField] private float rollForce;
    [SerializeField] private float delayBeforeInvincible;
    [SerializeField] private float invincibleLength;

    // Input and state variables
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float moveSpeed;

    private bool isGrounded;
    private bool isRunning;
    private bool canJump = true;
    private bool canRoll = true;
    private bool isAsleep;
    private bool doRoll;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStamina = GetComponent<PlayerStamina>();
        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        MyInput();
        SpeedControl();
        AnimationParameters();

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (isAsleep) return;

        if (Input.GetButtonDown("Roll") && canRoll && isGrounded)
        {
            canRoll = false;

            if (playerStamina.UseStamina(40f))
            {
                Roll();
                Invoke(nameof(StopRoll), delayBeforeInvincible + invincibleLength);
                Invoke(nameof(ResetRoll), rollCooldown);
            }
            else
            {
                Debug.Log("Not enough stamina to roll.");
                canRoll = true;
            }

            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        float inputMagnitude = new Vector2(horizontalInput, verticalInput).sqrMagnitude;
        isRunning = Input.GetButton("Run") && inputMagnitude > 0.1f && playerStamina.UseStamina(playerStamina.RunCost * Time.deltaTime);
        moveSpeed = isRunning ? runSpeed : walkSpeed;

        if (Input.GetButton("Jump") && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (!isRunning && playerStamina.CurrentStamina < playerStamina.MaxStamina)
        {
            playerStamina.RechargeStamina();
        }
    }

    private void MovePlayer()
    {
        if (doRoll)
        {
            rb.AddForce(10f * rollForce * playerObject.forward.normalized, ForceMode.Force);
            return;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else
        {
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    private void StopRoll()
    {
        doRoll = false;
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Roll()
    {
        playerHealth.Invincible(delayBeforeInvincible, invincibleLength);
        doRoll = true;
        animator.SetTrigger("Roll");
    }

    private void ResetRoll()
    {
        canRoll = true;
    }

    private void AnimationParameters()
    {
        animator.SetFloat("Velocity", rb.velocity.sqrMagnitude);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsFalling", rb.velocity.y < 0.01f);
        animator.SetBool("IsRunning", isRunning);
    }

    public void Sleep(float sleepTime)
    {
        StartCoroutine(SleepCoroutine(sleepTime));
    }

    private IEnumerator SleepCoroutine(float sleepTime)
    {
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

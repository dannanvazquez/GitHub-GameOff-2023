using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour {
    [Header("General Enemy References")]
    public Animator animator;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected ParticleSystem basicAttackParticles;
    [SerializeField] protected ParticleSystem specialAttackParticles;
    [SerializeField] protected SpecialAttackBase specialAttack;

    [Header("General Enemy Settings")]
    [Tooltip("The distance from the player before this enemy starts chasing.")]
    [SerializeField] protected float chasingRange;
    [Tooltip("The distance from the player before this enemy attempts to basic attack.")]
    [SerializeField] protected float basicAttackRange;
    [Tooltip("The amount of seconds in between each basic attack attempt.")]
    [SerializeField] protected float basicAttackCooldown;
    [Tooltip("The amount of damage a basic attack does to the player.")]
    [SerializeField] protected float basicAttackDamage;

    public NavMeshAgent agent;
    protected EnemyHealth health;
    protected PlayerHealth playerHealth;
    protected Rigidbody rb;
    protected Ice ice;

    [HideInInspector] public bool isBasicAttacking;
    [HideInInspector] public float lastTimeBasicAttacked;

    [HideInInspector] public bool isSpecialAttacking;
    [HideInInspector] public float lastTimeSpecialAttacked;

    [HideInInspector] public bool isFrozen;

    protected Node root;

    private void Awake() {
        if (playerTransform == null) playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
        ice = GetComponent<Ice>();
        lastTimeBasicAttacked -= basicAttackCooldown;
        if (specialAttack) lastTimeSpecialAttacked -= specialAttack.specialAttackCooldown;
    }

    private void Start() {
        rb.drag = 5f;

        ConstructBehaviorTree();
    }

    public virtual void ConstructBehaviorTree() {
        root = new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new Inverter(new IsAttackingNode(this)),
                new RangeNode(chasingRange, playerTransform, transform),
                new ChaseNode(animator, playerTransform, agent)
            })
        });
    }

    private void Update() {
        if (ice.isFrozen) return;

        root.Evaluate();

        if (root.nodeState == NodeState.FAILURE) {
            Idle();
        }
    }

    private void Idle() {
        animator.SetBool("IsWalking", false);
        if (agent.enabled) agent.isStopped = true;
    }

    public void DoneBasicAttacking() {
        isBasicAttacking = false;
    }

    public void BasicAttackParticles() {
        basicAttackParticles?.Play();
    }

    public void DoneSpecialAttacking() {
        isSpecialAttacking = false;
    }

    public void SpecialAttackParticles() {
        specialAttackParticles?.Play();
    }

    public void SetRotation(Quaternion rotation) {
        transform.rotation = rotation;
    }

    public void KnockbackEnemy(Vector3 force) {
        StartCoroutine(KnockbackCoroutine(force));
    }

    private IEnumerator KnockbackCoroutine(Vector3 force) {
        agent.enabled = false;
        rb.AddForce(force, ForceMode.Impulse);
        rb.drag = 0f;

        yield return new WaitForSeconds(0.5f);
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        while (!Physics.Raycast(transform.position, Vector3.down, 0.2f, groundMask)) yield return null;

        rb.drag = 5f;
        agent.enabled = true;
    }
}

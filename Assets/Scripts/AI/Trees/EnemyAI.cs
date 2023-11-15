using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    [Header("General Enemy References")]
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected ParticleSystem basicAttackParticles;

    [Header("General Enemy Settings")]
    [Tooltip("The distance from the player before this enemy starts chasing.")]
    [SerializeField] protected float chasingRange;
    [Tooltip("The distance from the player before this enemy attempts to basic attack.")]
    [SerializeField] protected float basicAttackRange;
    [Tooltip("The amount of seconds in between each basic attack attempt.")]
    [SerializeField] protected float basicAttackCooldown;
    [Tooltip("The amount of damage a basic attack does to the player.")]
    [SerializeField] protected float basicAttackDamage;

    protected NavMeshAgent agent;
    protected EnemyHealth health;
    protected Animator animator;
    protected PlayerHealth playerHealth;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public float lastTimeAttacked;

    protected Node root;

    private void Awake() {
        if (playerTransform == null) playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        lastTimeAttacked -= basicAttackCooldown;
    }

    private void Start() {
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
        root.Evaluate();

        if (root.nodeState == NodeState.FAILURE) {
            Idle();
        }
    }

    private void Idle() {
        animator.SetBool("IsWalking", false);
        agent.isStopped = true;
    }

    public void DoneAttacking() {
        isAttacking = false;
    }

    public void BasicAttackParticles() {
        basicAttackParticles?.Play();
    }
}

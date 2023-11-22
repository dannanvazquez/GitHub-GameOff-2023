using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI {
    [Header("Melee Enemy References")]
    [SerializeField] protected ParticleSystem specialAttackParticles;
    [SerializeField] protected SpecialAttackBase specialAttack;

    [Header("Melee Enemy Settings")]
    [Tooltip("The distance from the player that a melee hit will actually hit the player.")]
    [SerializeField] private float meleeDistance;

    public override void Awake() {
        base.Awake();
        if (specialAttack) lastTimeSpecialAttacked -= specialAttack.specialAttackCooldown;
    }

    public override void ConstructBehaviorTree() {
        OffCooldownNode specialAttackCooldownNode = new OffCooldownNode(this, specialAttack.specialAttackCooldown);
        OffCooldownNode meleeCooldownNode = new OffCooldownNode(this, basicAttackCooldown);

        root = new Sequence(new List<Node> {  // Does this meet the requirements in order to be aggressive?
            new Selector(new List<Node> {  // Do either of these requirements meet?
                new RangeNode(chasingRange, playerTransform, transform),
                new Inverter(new HealthMinThresholdNode(health, health.maxHealth))
            }),
            new Selector(new List<Node> {  // Decide the aggressive action that fits best.
                new Sequence(new List<Node> {  // Does this meet the requirements to rotate towards player?
                    new IsAttackingNode(this),
                    new RotateTowardsPlayerNode(this, playerTransform, transform)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements to special attack?
                    specialAttackCooldownNode,
                    new RangeNode(specialAttack.specialAttackRange, playerTransform, transform),
                    new Inverter(new RangeNode(8f, playerTransform, transform)),
                    new SpecialAttackNode(animator, agent, this, specialAttackCooldownNode, specialAttack)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements to basic attack?
                    meleeCooldownNode,
                    new RangeNode(basicAttackRange, playerTransform, transform),
                    new MeleeNode(animator, agent, this, meleeCooldownNode)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements to chase?
                    new Inverter(new RangeNode(minimumChaseRange, playerTransform, transform)),
                    new ChaseNode(animator, playerTransform, agent)
                }),
            }),
        });
    }

    public void MeleeAttack() {
        if (Vector3.Distance(transform.position, playerTransform.position) <= meleeDistance) {
            playerHealth.TakeDamage(basicAttackDamage);
        }
    }

    public void SpecialAttack() {
        specialAttack.PerformSpecialAttack();
    }

    public void SpecialAttackParticles() {
        if (specialAttackParticles) specialAttackParticles.Play();
    }
}

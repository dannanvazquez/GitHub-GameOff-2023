using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI {
    [Header("Melee Enemy Settings")]
    [Tooltip("The distance from the player that a melee hit will actually hit the player.")]
    [SerializeField] private float meleeDistance;

    public override void ConstructBehaviorTree() {
        OffCooldownNode specialAttackCooldownNode = new OffCooldownNode(this, specialAttack.specialAttackCooldown);
        OffCooldownNode meleeCooldownNode = new OffCooldownNode(this, basicAttackCooldown);

        root = new Selector(new List<Node> {
            new IsAttackingNode(this),
            new Sequence(new List<Node> {
                specialAttackCooldownNode,
                new RangeNode(specialAttack.specialAttackRange, playerTransform, transform),
                new Inverter(new RangeNode(8f, playerTransform, transform)),
                new SpecialAttackNode(animator, agent, this, specialAttackCooldownNode)
            }),
            new Sequence(new List<Node> {
                meleeCooldownNode,
                new RangeNode(basicAttackRange, playerTransform, transform),
                new MeleeNode(animator, agent, this, meleeCooldownNode)
            }),
            new Sequence(new List<Node> {
                new RangeNode(chasingRange, playerTransform, transform),
                new ChaseNode(animator, playerTransform, agent)
            })
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
}

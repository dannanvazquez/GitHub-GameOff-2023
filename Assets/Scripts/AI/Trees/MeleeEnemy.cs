using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI {
    [Header("Melee Enemy Settings")]
    [Tooltip("The distance from the player that a melee hit will actually hit the player.")]
    [SerializeField] private float meleeDistance;

    public override void ConstructBehaviorTree() {
        root = new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new OffCooldownNode(this, basicAttackCooldown),
                new RangeNode(basicAttackRange, playerTransform, transform),
                new MeleeNode(animator, agent, this, playerTransform.GetComponent<PlayerHealth>())
            }),
            new Sequence(new List<Node> {
                new Inverter(new IsAttackingNode(this)),
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
}

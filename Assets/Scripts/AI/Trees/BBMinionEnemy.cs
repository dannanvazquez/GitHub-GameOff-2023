using System.Collections.Generic;
using UnityEngine;

public class BBMinionEnemy : EnemyAI {
    [Header("BBBoss Enemy References")]
    [SerializeField] private GameObject sword;

    [Header("BBBoss Enemy Settings")]
    [Tooltip("The distance from the player that a melee hit will actually hit the player.")]
    [SerializeField] private float meleeDistance;

    public override void ConstructBehaviorTree() {
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
                new Sequence(new List<Node> {  // Does this meet the requirements to close range combat?
                    new RangeNode(basicAttackRange, playerTransform, transform),
                    meleeCooldownNode,
                    new MeleeNode(animator, agent, this, meleeCooldownNode)
                }),
                new ChaseNode(animator, playerTransform, agent)
            })
        });
    }

    public void MeleeAttack() {
        if (Vector3.Distance(transform.position, playerTransform.position) <= meleeDistance) {
            playerHealth.TakeDamage(basicAttackDamage);
        }
    }

    public void ToggleSword(int toggle) {
        sword.SetActive(toggle != 0);
    }
}

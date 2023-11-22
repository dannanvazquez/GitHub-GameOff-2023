using System.Collections.Generic;
using UnityEngine;

public class BBBossEnemy : EnemyAI {
    [Header("BBBoss Enemy References")]
    [SerializeField] private GameObject sword;
    [SerializeField] protected SpecialAttackBase mediumRangeAttack;
    [SerializeField] protected SpecialAttackBase farRangeAttack;

    [Header("BBBoss Enemy Settings")]
    [Tooltip("The distance from the player that a melee hit will actually hit the player.")]
    [SerializeField] private float meleeDistance;

    public override void ConstructBehaviorTree() {
        OffCooldownNode meleeCooldownNode = new OffCooldownNode(this, basicAttackCooldown);
        OffCooldownNode mediumAttackCooldownNode = new OffCooldownNode(this, mediumRangeAttack.specialAttackCooldown);
        OffCooldownNode farAttackCooldownNode = new OffCooldownNode(this, farRangeAttack.specialAttackCooldown);

        root = new Sequence(new List<Node> {  // Does this meet the requirements in order to be aggressive?
            new Selector(new List<Node> {  // Do either of these requirements meet?
                new RangeNode(chasingRange, playerTransform, transform),
                new Sequence(new List<Node> {
                    // Enable anti-range shield here
                    new Inverter(new HealthMinThresholdNode(health, health.maxHealth))
                })
            }),
            new Selector(new List<Node> {  // Decide the aggressive action that fits best.
                new Sequence(new List<Node> {  // Does this meet the requirements to rotate towards player?
                    new IsAttackingNode(this),
                    new RotateTowardsPlayerNode(this, playerTransform, transform)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements for close range combat?
                    new RangeNode(basicAttackRange, playerTransform, transform),
                    new Selector(new List<Node> {  // Decide the close combat action that best fits
                        new Sequence(new List<Node> {  // Does this meet the requirements for a close range attack?
                            meleeCooldownNode,
                            new MeleeNode(animator, agent, this, meleeCooldownNode)
                        }),
                        new ChaseNode(animator, playerTransform, agent)
                    })
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements for medium range combat?
                    new RangeNode(mediumRangeAttack.specialAttackRange, playerTransform, transform),
                    new Selector(new List<Node> {  // Decide the medium combat action that best fits
                        new Sequence(new List<Node> {  // Does this meet the requirements for a medium range attack?
                            mediumAttackCooldownNode,
                            new SpecialAttackNode(animator, agent, this, mediumAttackCooldownNode, mediumRangeAttack)
                        }),
                        new ChaseNode(animator, playerTransform, agent)
                    })
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements for far range combat?
                    new RangeNode(farRangeAttack.specialAttackRange, playerTransform, transform),
                    farAttackCooldownNode,
                    new SpecialAttackNode(animator, agent, this, farAttackCooldownNode, farRangeAttack)
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

    public void MediumSpecialAttack() {
        mediumRangeAttack.PerformSpecialAttack();
    }

    public void FarSpecialAttack() {
        farRangeAttack.PerformSpecialAttack();
    }

    public void ToggleSword(int toggle) {
        sword.SetActive(toggle != 0);
    }
}

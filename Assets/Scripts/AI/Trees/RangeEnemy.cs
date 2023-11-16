using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyAI {
    [Header("Range Enemy References")]
    [SerializeField] private Transform projectileSpawnTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject loadedProjectile;

    [Header("Range Enemy Settings")]
    [Tooltip("The initial force that the projectile of the basic attack is thrown at.")]
    [SerializeField] private float projectileForce;

    public override void ConstructBehaviorTree() {
        OffCooldownNode shootCooldownNode = new OffCooldownNode(this, basicAttackCooldown);

        root = new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new IsAttackingNode(this),
                new RotateTowardsPlayerNode(this, playerTransform, transform)
            }),
            new Sequence(new List<Node> {
                shootCooldownNode,
                new RangeNode(basicAttackRange, playerTransform, transform),
                new ShootNode(animator, agent, this, shootCooldownNode)
            }),
            new Sequence(new List<Node> {
                new RangeNode(chasingRange, playerTransform, transform),
                new ChaseNode(animator, playerTransform, agent)
            })
        });
    }

    public IEnumerator LoadProjectile() {
        yield return new WaitForSeconds(basicAttackCooldown);
        loadedProjectile = Instantiate(projectilePrefab, projectileSpawnTransform);
    }

    public void RangeAttack() {
        loadedProjectile.transform.parent = null;
        loadedProjectile.transform.rotation = Quaternion.LookRotation((playerTransform.position - loadedProjectile.transform.position).normalized);
        loadedProjectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        loadedProjectile.GetComponent<Rigidbody>().AddForce(loadedProjectile.transform.forward * projectileForce, ForceMode.Impulse);
        loadedProjectile = null;
    }

    public void SpecialAttack() {
        specialAttack.PerformSpecialAttack();
    }
}

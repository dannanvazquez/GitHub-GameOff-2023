using UnityEngine;
using UnityEngine.AI;

public class DuplicateAttack : SpecialAttackBase {
    [Header("Duplicate Attack References")]
    [SerializeField] private GameObject enemyPrefab;

    private EnemyAI ai;

    [Header("Duplicate Attack Settings")]
    [Tooltip("The distance from this enemy that the duplicated enemies spawn at.")]
    [SerializeField] private float spawnDistance;
    [Tooltip("The amount of duplicated enemies spawned.")]
    [SerializeField] private float duplicateAmount;
    [Tooltip("The percentage of HP the duplicated enemy spawns with.")]
    [SerializeField, Range(0, 1)] private float initialHealth;
    // Particles
    [SerializeField] private GameObject duplicateParticlesPrefab;
    private void Awake() {
        ai = GetComponent<EnemyAI>();
    }


    public override void StartSpecialAttack() {
        ai.animator.SetTrigger("DuplicateAttack");
    }

    public override void PerformSpecialAttack() {
        for (int i = 0; i < duplicateAmount; i++) {
            DuplicateEnemy();
        }
    }

    private void DuplicateEnemy() {
        Quaternion direction = Quaternion.LookRotation((ai.playerTransform.position - transform.position).normalized);
        direction.x = transform.rotation.x;
        direction.z = transform.rotation.z;
        GameObject duplicatedEnemy = Instantiate(enemyPrefab, RandomPoint(), direction);

        Instantiate(duplicateParticlesPrefab, duplicatedEnemy.transform.position, Quaternion.identity);
        if (duplicatedEnemy.TryGetComponent(out EnemyHealth health)) {
            health.TakeDamage(health.maxHealth * (1 - initialHealth));
        }
    }

    private Vector3 RandomPoint() {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnDistance;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f * i, NavMesh.AllAreas)) {
                return hit.position;
            }
        }

        return transform.position;
    }
}

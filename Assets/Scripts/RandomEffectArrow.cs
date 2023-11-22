using UnityEngine;

public class RandomEffectArrow : BasicArrow {
    [Header("Random Effect References")]
    [SerializeField] private GameObject blackholeParticlePrefab;

    [Header("Random Effect Settings")]
    [Tooltip("The range around the arrow where enemies are moved closer to the player.")]
    [SerializeField] private float enemyCloserRange;
    [Tooltip("The ratio between initial distance to the player the enemies will move. 0 is not moving at all, while 1 is on top of the player.")]
    [SerializeField, Range(0, 1)] private float enemyCloserDistanceRatio;
    [Tooltip("The range around the arrow where enemies are sucked into the void.")]
    [SerializeField] private float blackHoleRange;

    public override void OnHit(Collider target) {
        int randomAbility = Random.Range(0, 3);
        switch (randomAbility) {
            case 0:
                OneHitKill(target);
                break;
            case 1:
                TeleportEnemiesCloser();
                break;
            case 2:
                BlackHole();
                break;
            default:
                break;
        }
    }
    
    private void OneHitKill(Collider target) {
        if (target.TryGetComponent(out EnemyHealth health)) {
            health.TakeDamage(health.maxHealth);
        }
    }

    private void TeleportEnemiesCloser() {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] enemies = Physics.OverlapSphere(transform.position, enemyCloserRange, enemyLayerMask);

        foreach (var enemy in enemies) {
            if (enemy.TryGetComponent(out EnemyAI ai)) {
                enemy.transform.position = enemy.transform.position + (ai.playerTransform.position - enemy.transform.position) * enemyCloserDistanceRatio;
            }
        }
    }

    private void BlackHole() {
        Instantiate(blackholeParticlePrefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] enemies = Physics.OverlapSphere(transform.position, blackHoleRange, enemyLayerMask);

        foreach (var enemy in enemies) {
            enemy.transform.position = transform.position;

            if (enemy.TryGetComponent(out EnemyHealth enemyHealth)) {
                enemyHealth.TakeDamage(enemyHealth.maxHealth);
            }
        }
    }
}
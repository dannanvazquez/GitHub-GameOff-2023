using UnityEngine;

public class BerryBomb : EnemyProjectile {
    [Header("Berry Bomb Settings")]
    [Tooltip("The radius of the explosion when collided.")]
    [SerializeField] private float explosionRadius;
    [Tooltip("The amount of damage the explosion will do to the player.")]
    [SerializeField] private float explosionDamage;
    [Tooltip("The amount of healing the explosion will do to its allies.")]
    [SerializeField] private float explosionHeal;

    protected override void OnHit() {
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRadius, playerLayerMask);

        foreach (var player in players) {
            if (player.transform.parent.TryGetComponent(out PlayerHealth health)) {
                health.TakeDamage(explosionDamage);
            }
        }

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);

        foreach (var enemy in enemies) {
            if (enemy.transform.TryGetComponent(out EnemyHealth health)) {
                health.Heal(explosionHeal);
            }
        }
    }
}

using UnityEngine;

public class ExplosiveArrow : BasicArrow {
    [Header("Explosive Arrow References")]
    [SerializeField] private ParticleSystem hitParticles;

    [Header("Explosive Arrow Settings")]
    [Tooltip("The amount of damage the explosion does.")]
    [SerializeField] private float explosionDamage;
    [Tooltip("The radius of the explosion.")]
    [SerializeField] private float explosionRadius;
    [Tooltip("The force the explosion deals on entities.")]
    [SerializeField] private float explosionKnockbackForce;

    public override void OnHit(Collider target) {
        hitParticles.Play();

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);

        foreach (var enemy in enemies) {
            float efficency = (explosionRadius - Vector3.Distance(transform.position, enemy.transform.position) / explosionRadius);

            if (enemy.TryGetComponent(out EnemyAI ai) || enemy.transform.root.TryGetComponent(out ai)) {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                direction.y = explosionKnockbackForce;
                ai.KnockbackEnemy(efficency * explosionKnockbackForce * direction);
            }

            if (enemy.TryGetComponent(out EnemyHealth enemyHealth) || enemy.transform.root.TryGetComponent(out enemyHealth)) {
                enemyHealth.TakeDamage(efficency * explosionDamage);
            }
        }
    }
}

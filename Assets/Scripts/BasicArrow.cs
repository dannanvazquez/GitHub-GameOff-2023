using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BasicArrow : MonoBehaviour {
    private Rigidbody rb;

    [Header("Settings")]
    [Tooltip("The amount of damage an arrow hit does to an enemy.")]
    [SerializeField] private float damage;

    private bool hasHit;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Destroy(gameObject, 10);
    }

    private void Update() {
        if (rb != null && rb.velocity != Vector3.zero) {
            transform.forward = rb.velocity;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (hasHit) return;

        hasHit = true;

        if (other.transform.root.TryGetComponent(out EnemyHealth enemyHealth)) {
            enemyHealth.TakeDamage(damage);
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}

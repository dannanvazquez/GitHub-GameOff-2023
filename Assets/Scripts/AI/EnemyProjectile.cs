using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour {
    private Rigidbody rb;

    [HideInInspector] public float damage;

    private bool hasHit;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Destroy(gameObject, 25);
    }

    private void Update() {
        if (rb != null && rb.velocity != Vector3.zero) {
            transform.forward = rb.velocity;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (hasHit) return;

        hasHit = true;

        if (other.transform.parent.TryGetComponent(out PlayerHealth playerHealth)) {
            playerHealth.TakeDamage(damage);
        }
        OnHit();

        Destroy(gameObject);
    }

    protected virtual void OnHit() { }
}

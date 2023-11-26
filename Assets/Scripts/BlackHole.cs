using UnityEngine;

public class BlackHole : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The radius from this black hole to pull enemies from.")]
    [SerializeField] private float pullRadius;
    [Tooltip("The maximum amount of force this black hole will pull in.")]
    [SerializeField] private float gravitationalPull; // Pull force
    /*[Tooltip("The maximum distance from the black hole that it will destroy enemies.")]
    [SerializeField] private float destroyRadius;*/

    [SerializeField] private LayerMask layersToPull;

    private Transform childTransform;  // Reference to the particle system that will destroy on its own.

    private void Awake() {
        childTransform = transform.GetChild(0);
    }

    private void FixedUpdate() {
        if (childTransform == null) Destroy(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius, layersToPull);

        foreach (var collider in colliders) {
            if (!collider.TryGetComponent(out EnemyAI ai)) continue;

            if (!collider.TryGetComponent(out Rigidbody rb)) {
                rb = collider.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;

                ai.agent.enabled = false;
                rb.freezeRotation = true;
                rb.useGravity = false;
                rb.drag = 5;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            float gravityIntensity = pullRadius / distance;
            Vector3 direction = transform.position - collider.transform.position;

            rb.AddForce(gravitationalPull * gravityIntensity * rb.mass * Time.fixedDeltaTime * direction);

            /*if (distance < destroyRadius) {
                Destroy(collider.gameObject);
            }*/
        }
    }

    private void OnDestroy() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius, layersToPull);

        foreach (var collider in colliders) {
            if (!collider.TryGetComponent(out EnemyAI ai) || !collider.TryGetComponent(out Rigidbody rb)) continue;

            Destroy(rb);
            ai.agent.enabled = true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageVisualsInterval;
    [SerializeField, Range(0, 1f)] private float minimumScalePercentage;

    private float currentHealth;
    private Vector3 initialScale;

    private void Awake() {
        currentHealth = maxHealth;
        initialScale = transform.localScale;
    }

    public bool TakeDamage(float damage) {
        if (currentHealth <= 0) return true;

        currentHealth -= damage;
        StartCoroutine(DamageVisuals());
        if (currentHealth <= 0) {
            Debug.Log($"{gameObject.name} is now dead", transform);
            Destroy(GetComponent<EnemyAI>());
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<EnemyAI>().animator.SetTrigger("Death");
            return true;
        }
        Debug.Log($"{gameObject.name} is now at " + currentHealth + " health.", transform);
        return false;
    }

    public void Die() {
        Destroy(gameObject);
    }

    private IEnumerator DamageVisuals() {
        transform.localScale = initialScale * ((1f - minimumScalePercentage) * (currentHealth / maxHealth) + minimumScalePercentage);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) {
            foreach (var m in r.materials) {
                m.color = Color.red;
            }
        }

        yield return new WaitForSeconds(damageVisualsInterval);

        foreach (var r in renderers) {
            foreach (var m in r.materials) {
                m.color = Color.white;
            }
        }
    }
}

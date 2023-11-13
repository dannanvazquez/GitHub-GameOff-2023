using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth;

    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
            Debug.Log($"{gameObject.name} is now dead", transform);
            return true;
        }
        Debug.Log($"{gameObject.name} is now at " + currentHealth + " health.", transform);
        return false;
    }

    private void Die() {
        Destroy(gameObject);
    }
}

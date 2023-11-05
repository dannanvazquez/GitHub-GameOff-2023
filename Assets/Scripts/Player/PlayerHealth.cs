using UnityEngine;

public class PlayerHealth : MonoBehaviour {
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
            Debug.Log("Player is now dead");
            return true;
        }
        Debug.Log("Player is now at " + currentHealth + " health.");
        return false;
    }

    private void Die() {
        Destroy(gameObject);
    }
}
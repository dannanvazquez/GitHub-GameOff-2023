using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth;

    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(float damage) {
        if (currentHealth <= 0) return true;

        currentHealth -= damage;
        if (currentHealth <= 0) {
            Debug.Log($"{gameObject.name} is now dead", transform);
            Destroy(GetComponent<EnemyAI>());
            GetComponent<Animator>().SetTrigger("Death");
            return true;
        }
        Debug.Log($"{gameObject.name} is now at " + currentHealth + " health.", transform);
        return false;
    }

    public void Die() {
        Destroy(gameObject);
    }
}

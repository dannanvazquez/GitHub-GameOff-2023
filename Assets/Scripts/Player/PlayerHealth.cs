using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private float maxHealth = 1.0f;
    private float maxStamina = 1.0f;


    private float currentHealth;
    private float currentStamina;
 
    [SerializeField]
    private UnityEngine.UI.Image _healthBarForegroundImage;

    [SerializeField]
    private UnityEngine.UI.Image _staminaBarForegroundImage;

    private void Awake() {
        currentHealth = 0.7f;
        currentStamina = 0.7f;
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

    public bool LoseStamina(float staminaLost)
    {
        currentStamina -= staminaLost;
        if (currentStamina <= 0)
        {
            Debug.Log("Player has no stamina");
            return true;
        }
        Debug.Log("Player is now at " + currentStamina + " stamina.");
        return false;
    }

    private void Die() {
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        UpdateHealthAndStamina();
    }

    private void UpdateHealthAndStamina()
    {
        if (_healthBarForegroundImage.fillAmount != currentHealth)
            _healthBarForegroundImage.fillAmount = currentHealth;

        if (_staminaBarForegroundImage.fillAmount != currentStamina)
            _staminaBarForegroundImage.fillAmount = currentStamina;
    }
}
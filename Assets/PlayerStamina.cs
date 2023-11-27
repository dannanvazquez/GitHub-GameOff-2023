// PlayerStamina.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float runCost;
    [SerializeField] private float chargeRate;

    private float currentStamina;

    [SerializeField]
    private Image staminaBar;

    private Coroutine recharge;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            UpdateStaminaBar();
            return true;
        }
        return false;
    }

    public void RechargeStamina()
    {
        if (recharge == null)
        {
            recharge = StartCoroutine(RechargeStaminaCoroutine());
        }
    }

    private IEnumerator RechargeStaminaCoroutine()
    {
        while (currentStamina < maxStamina)
        {
            currentStamina += chargeRate * Time.deltaTime;
            currentStamina = Mathf.Min(maxStamina, currentStamina);
            UpdateStaminaBar();
            yield return null;
        }
        recharge = null;
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    // Add getters for the properties
    public float RunCost => runCost;
    public float MaxStamina => maxStamina;
    public float ChargeRate => chargeRate;
    public float CurrentStamina => currentStamina;
}

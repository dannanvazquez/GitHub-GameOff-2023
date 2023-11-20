using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("References")]
    [SerializeField] private PlayerCamera playerCamera;

    [Header("Settings")]
    [Tooltip("The amount of seconds the damage visuals are shown for.")]
    [SerializeField] private float damageVisualsInterval;

    private float maxHealth = 100.0f;
    private float maxStamina = 1.0f;

    private float currentHealth;
    private float currentStamina;

    [SerializeField]
    private UnityEngine.UI.Image _healthBarForegroundImage;

    [SerializeField]
    private UnityEngine.UI.Image _staminaBarForegroundImage;

    [Header("Sounds")]
    [SerializeField] private AudioSource damage_audioSource;
    [SerializeField] private AudioSource voice_audioSource;
    [Tooltip("Getting hit")]
    [SerializeField] private AudioClip[] hit_sfx;
    [SerializeField] private AudioClip[] hit_voice;    
        private AudioClip lasthitClip;  
        private AudioClip lasthitvoiceClip;  

        // SOUNDS //
      private void PlayRandomClip(AudioClip[] clips, ref AudioClip lastClip, AudioSource audioSource)
    {
        if (clips.Length > 0)
        {
            AudioClip clip;
            do
            {
                clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            } while (clip == lastClip);

            lastClip = clip;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    int RandomExcept(int except, int max)
    {
        if (max <= 0)
        {
            return -1; // or some default value indicating an error
        }

        int result;
        do
        {
        result = UnityEngine.Random.Range(0, max);
        } while (result == except);
        return result;
    }

    private void Awake() {
        currentHealth = maxHealth * 0.7f;
        currentStamina = 0.7f;
    }

    public bool TakeDamage(float damage) {
        currentHealth -= damage;
        if (damage > 0) StartCoroutine(DamageVisuals());
        if (currentHealth <= 0) {
            Die();
            Debug.Log("Player is now dead");
            return true;
        }
        Debug.Log("Player is now at " + currentHealth + " health.");
        PlayRandomClip(hit_sfx, ref lasthitClip, damage_audioSource);
        PlayRandomClip(hit_voice, ref lasthitvoiceClip, voice_audioSource);
        return false;
    }

    public bool LoseStamina(float staminaLost) {
        currentStamina -= staminaLost;
        if (currentStamina <= 0) {
            Debug.Log("Player has no stamina");
            return true;
        }
        Debug.Log("Player is now at " + currentStamina + " stamina.");
        return false;
    }

    private void Die() {
        PlayerCombat playerCombat = GetComponent<PlayerCombat>();
        playerCombat.animator.SetTrigger("Death");
        playerCombat.enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        playerCamera.enabled = false;
    }

    void LateUpdate() {
        UpdateHealthAndStamina();
    }

    private void UpdateHealthAndStamina() {
        if (_healthBarForegroundImage && _healthBarForegroundImage.fillAmount != currentHealth)
            _healthBarForegroundImage.fillAmount = currentHealth;

        if (_staminaBarForegroundImage && _staminaBarForegroundImage.fillAmount != currentStamina)
            _staminaBarForegroundImage.fillAmount = currentStamina;
    }

    private IEnumerator DamageVisuals() {
        playerCamera.ShakeCamera(5f, 0.2f);

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
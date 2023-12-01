using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private GameObject deathParticlePrefab;

    [Header("Settings")]
    [Tooltip("The amount of seconds the damage visuals are shown for.")]
    [SerializeField] private float damageVisualsInterval;

    public float maxHealth = 100.0f;
    public float currentHealth;
    private bool isInvincible;

    [SerializeField]
    UnityEngine.UI.Image healthBarForegroundImage;

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

    private void Awake()
    {
        currentHealth = maxHealth * 0.7f;
    }

    // Returns true if the player is dead.
    public bool TakeDamage(float damage)
    {
        if (currentHealth <= 0) return true;
        if (isInvincible) return false;

        currentHealth -= damage;
        if (damage > 0) StartCoroutine(DamageVisuals());
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        PlayRandomClip(hit_sfx, ref lasthitClip, damage_audioSource);
        PlayRandomClip(hit_voice, ref lasthitvoiceClip, voice_audioSource);

        // Update health bar
        UpdateHealthBar();

        return false;
    }

    public void Heal(float healAmount) {
        if (healAmount <= 0 || currentHealth >= maxHealth) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        StartCoroutine(HealVisuals());
    }

    private void Die()
    {
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        PlayerCombat playerCombat = GetComponent<PlayerCombat>();
        playerCombat.animator.SetBool("IsDead", true);
        playerCombat.enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        playerCamera.enabled = false;

    }
    void Update(){
                if (currentHealth <= 0)
        {
            // Player is dead, allow respawn with "R" key
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }
    }
    void Respawn()
    {
    StartCoroutine(RespawnWithDelay());
    }


    private IEnumerator RespawnWithDelay()
{
    // Add a delay before respawning (adjust the duration as needed)
    yield return new WaitForSeconds(3f);
    currentHealth = maxHealth;
    isInvincible = false;
    GameManager.Instance.RespawnPlayer(gameObject);
    PlayerCombat playerCombat = GetComponent<PlayerCombat>();
    playerCombat.animator.SetBool("IsDead", false);
    playerCombat.enabled = true;
    GetComponent<PlayerMovement>().enabled = true;
    playerCamera.enabled = true;
}
    void LateUpdate()
    {
        // Update health bar in LateUpdate to ensure proper synchronization
        UpdateHealthBar();
    
    }

    

    public void UpdateHealthBar()
    {
        if (healthBarForegroundImage && healthBarForegroundImage.fillAmount != currentHealth / maxHealth)
        {
            healthBarForegroundImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private IEnumerator DamageVisuals()
    {
        playerCamera.ShakeCamera(5f, 0.2f);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            foreach (var m in r.materials)
            {
                m.color = Color.red;
            }
        }

        yield return new WaitForSeconds(damageVisualsInterval);

        foreach (var r in renderers)
        {
            foreach (var m in r.materials)
            {
                m.color = Color.white;
            }
        }
    }

    private IEnumerator HealVisuals() {
        //transform.localScale = initialScale * ((1f - minimumScalePercentage) * (currentHealth / maxHealth) + minimumScalePercentage);

        UpdateHealthBar();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) {
            foreach (var m in r.materials) {
                m.color = Color.green;
            }
        }

        yield return new WaitForSeconds(damageVisualsInterval);

        foreach (var r in renderers) {
            foreach (var m in r.materials) {
                m.color = Color.white;
            }
        }
    }

    public void Invincible(float delay, float invincibleLength) {
        StartCoroutine(InvincibleCoroutine(delay, invincibleLength));
    }

    private IEnumerator InvincibleCoroutine(float delay, float invincibleLength) {
        yield return new WaitForSeconds(delay);

        isInvincible = true;

        yield return new WaitForSeconds(invincibleLength);

        isInvincible = false;
    }
}
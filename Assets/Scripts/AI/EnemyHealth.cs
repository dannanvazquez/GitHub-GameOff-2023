using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private RectTransform healthRect;
    [SerializeField] private GameObject hitParticlesPrefab;
    [SerializeField] private Transform enemyObjectTransform;
    [SerializeField] private Vector3 particleSpawnOffset;  // This spawns particles offset from enemyObjectTransform's position.
    [SerializeField] private GameObject deathParticlePrefab;

    private float healthRectWidth;

    [Header("Settings")]
    public float maxHealth;
    [SerializeField] private float damageVisualsInterval;
    [SerializeField, Range(0, 1f)] private float minimumScalePercentage;

    [HideInInspector] public float currentHealth;
    private Vector3 initialScale;

    [Header("Sounds")]
    [SerializeField] private AudioSource damage_audioSource;
        [SerializeField] private AudioSource damage_voice_audioSource;
    [Tooltip("Getting hit")]
    [SerializeField] private AudioClip[] hit_sfx;
    [SerializeField] private AudioClip[] hit_voice;
    [Tooltip("Getting killed")]
    [SerializeField] private AudioClip[] death_sfx;
    private AudioClip lasthitClip;    
        private AudioClip lastvoicehitClip;    
    private AudioClip lastdeathClip;

    [Header("Events")]
    [SerializeField] private UnityEvent onDamage;

    //public string Broken { get; private set; }
        private void PlayRandomClipNoLast(AudioClip[] clips, AudioSource audioSource)
    {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch=UnityEngine.Random.Range(.95f, 1.05f);
            audioSource.Play();
    }
    // SOUNDS //
    private void PlayRandomClip(AudioClip[] clips, ref AudioClip lastClip, AudioSource audioSource)
    {
        if (clips == null || clips.Length == 0 || audioSource == null)
        {
            Debug.LogError("AudioClip array or AudioSource is null or empty.");
            return;
        }

        AudioClip clip;
        do
        {
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        } while (clip == lastClip);

        lastClip = clip;
        audioSource.clip = clip;
        audioSource.Play();
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
        currentHealth = maxHealth;
        initialScale = transform.localScale;
        healthRectWidth = healthRect.rect.width;
        healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);
        if (healthFillImage) healthFillImage.fillAmount = currentHealth / maxHealth;
    }

     public bool TakeDamage(float damage) {
        if (currentHealth <= 0) return true;

        currentHealth -= damage;
        onDamage?.Invoke();
        if (damage > 0) StartCoroutine(DamageVisuals());
        if (currentHealth <= 0) {
            Die();
            return true;
        }
        PlayRandomClipNoLast(hit_sfx, damage_voice_audioSource);
        PlayRandomClipNoLast(hit_voice, damage_audioSource);
        damage_audioSource.volume = UnityEngine.Random.Range(.19f, .28f);
        damage_audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);
        return false;
    }

    public void Die() {
        Instantiate(deathParticlePrefab, enemyObjectTransform.position + particleSpawnOffset, Quaternion.identity);
        //GetComponent<EnemyAI>().animator.SetTrigger("Death");
        PlayRandomClip(death_sfx, ref lastdeathClip, damage_audioSource);
        Destroy(this.gameObject);
    }

    public void Heal(float healAmount) {
        if (healAmount <= 0 || currentHealth >= maxHealth) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        StartCoroutine(HealVisuals());
    }

    private IEnumerator DamageVisuals() {
        //transform.localScale = initialScale * ((1f - minimumScalePercentage) * (currentHealth / maxHealth) + minimumScalePercentage);

        // Update the fill amount instead of resizing the RectTransform.
        healthFillImage.fillAmount = currentHealth / maxHealth;
        //healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);

        Instantiate(hitParticlesPrefab, enemyObjectTransform.position + particleSpawnOffset, Quaternion.identity);

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

    private IEnumerator HealVisuals() {
        //transform.localScale = initialScale * ((1f - minimumScalePercentage) * (currentHealth / maxHealth) + minimumScalePercentage);

        healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);

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
}

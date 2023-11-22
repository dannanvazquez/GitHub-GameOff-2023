using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private RectTransform healthRect;
    [SerializeField] private GameObject hitParticlesPrefab;
    [SerializeField] private Transform enemyObjectTransform;

    private float healthRectWidth;

    [Header("Settings")]
    public float maxHealth;
    [SerializeField] private float damageVisualsInterval;
    [SerializeField, Range(0, 1f)] private float minimumScalePercentage;

    [HideInInspector] public float currentHealth;
    private Vector3 initialScale;
    [Header("Sounds")]
    [SerializeField] private AudioSource damage_audioSource;
    [Tooltip("Getting hit")]
    [SerializeField] private AudioClip[] hit_sfx;
    [Tooltip("Getting killed")]
    [SerializeField] private AudioClip[] death_sfx;
    private AudioClip lasthitClip;    
    private AudioClip lastdeathClip;

    //public string Broken { get; private set; }

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
        currentHealth = maxHealth;
        initialScale = transform.localScale;
        healthRectWidth = healthRect.rect.width;
        healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);
        if (healthFillImage) healthFillImage.fillAmount = currentHealth / maxHealth;

        
    }

     public bool TakeDamage(float damage) {
        if (currentHealth <= 0) return true;

        currentHealth -= damage;
        if (damage > 0) StartCoroutine(DamageVisuals());
        if (currentHealth <= 0) {
            Debug.Log($"{gameObject.name} is now dead", transform);
            Destroy(GetComponent<EnemyAI>());
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent.enabled) agent.isStopped = true;
            GetComponent<EnemyAI>().animator.SetTrigger("Death");
            PlayRandomClip(death_sfx, ref lastdeathClip, damage_audioSource);
            return true;
        }
        PlayRandomClip(hit_sfx, ref lasthitClip, damage_audioSource);
        damage_audioSource.volume = UnityEngine.Random.Range(.19f, .28f);
        damage_audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);
        return false;
    }

    public void Die() {
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

        Instantiate(hitParticlesPrefab, enemyObjectTransform.position, Quaternion.identity);

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

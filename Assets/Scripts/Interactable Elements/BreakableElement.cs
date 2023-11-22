using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BreakableElement : MonoBehaviour
{
    public GameObject BoxWithRB;
    public GameObject BoxNoRB;

    private Animator animator;


    [Header("References")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private RectTransform healthRect;

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

    private void Awake()
    {
        currentHealth = maxHealth;
        initialScale = transform.localScale;
        healthRectWidth = healthRect.rect.width;
        healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);
        healthFillImage.fillAmount = currentHealth / maxHealth;


    }

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetBool("Broken", false);
    }

    public bool TakeDamage(float damage)
    {
        Debug.Log("dmg taken");
        if (currentHealth <= 0) return true;

        currentHealth -= damage;
        if (damage > 0) StartCoroutine(DamageVisuals());
        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} is now dead", transform);








            BoxNoRB.SetActive(false);
            BoxWithRB.SetActive(true);
            animator.SetBool("Broken", true);

            Destroy(this.gameObject, 12f);







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

    public void Die()
    {
        Destroy(this.gameObject);
    }


    private IEnumerator DamageVisuals()
    {
        //transform.localScale = initialScale * ((1f - minimumScalePercentage) * (currentHealth / maxHealth) + minimumScalePercentage);

        // Update the fill amount instead of resizing the RectTransform.
        healthFillImage.fillAmount = currentHealth / maxHealth;
        //healthRect.sizeDelta = new Vector2(currentHealth / maxHealth * healthRectWidth, healthRect.sizeDelta.y);

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
}

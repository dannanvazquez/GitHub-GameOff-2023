using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BasicArrow : MonoBehaviour {
    private Rigidbody rb;

    [Header("Settings")]
    [Tooltip("The amount of damage an arrow hit does to an enemy.")]
    [SerializeField] private float damage;
    [Header("AudioClips")]
    [Tooltip("Arrow shoot sfx")]
    [SerializeField] private AudioClip[] arrow_sfx;
    [Tooltip("Arrow touched an obstacle sfx")]
    [SerializeField] private AudioClip[] arrow_landing_sfx;
    [SerializeField] private AudioSource AudioSource_arrow;

    [HideInInspector] public Vector3 hitPos;
    private bool hasHit;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        PlayRandomClip(arrow_sfx, AudioSource_arrow);
    }

    private void Start() {
        Destroy(gameObject, 10);
    }

    private void Update() {
        if (rb != null && rb.velocity != Vector3.zero) {
            transform.forward = rb.velocity;
        }
    }

    private void PlayRandomClip(AudioClip[] clips, AudioSource audioSource)
    {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch = UnityEngine.Random.Range(.85f, 1.05f);
            audioSource.Play();
            
    }

    private void OnTriggerEnter(Collider other) {
        if (hasHit) return;

        transform.position = hitPos;
        GameObject emptyGameObject = new GameObject("Arrow Parent");
        emptyGameObject.transform.SetParent(other.transform, true);
        transform.SetParent(emptyGameObject.transform, true);
        hasHit = true;
        PlayRandomClip(arrow_landing_sfx, AudioSource_arrow);

        AntiRangeShieldAttack antiRangeShieldAttack = null;
        if (other.transform.TryGetComponent(out EnemyHealth enemyHealth) && (!other.transform.TryGetComponent(out antiRangeShieldAttack) || !antiRangeShieldAttack.shieldActive)) {
            enemyHealth.TakeDamage(damage);
        } else if (other.transform.TryGetComponent(out BreakableElement breakableElement)) {
            breakableElement.TakeDamage(1);
        }
        if (antiRangeShieldAttack == null || !antiRangeShieldAttack.shieldActive) {
            OnHit(other);
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void OnHit(Collider target) {

    }

    private void OnDestroy() {
        if (transform.parent) {
            Destroy(transform.parent.gameObject);
        }
    }
}

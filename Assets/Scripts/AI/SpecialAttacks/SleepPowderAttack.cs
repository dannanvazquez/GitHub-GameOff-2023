using UnityEngine;

public class SleepPowderAttack : SpecialAttackBase {
    [Header("Sleep Powder References")]
    [SerializeField] private GameObject sleepPowderPrefab;
    [SerializeField] private Transform sleepPowderSpawnTransform;

    private EnemyAI ai;
    private GameObject sleepPowder;

    [SerializeField] private AudioSource sleepattack_audioSource;
    [SerializeField] private AudioClip spawn_sleep_powder_sfx;
    [SerializeField] private AudioClip throw_sleep_powder_sfx;    


    [Header("Sleep Powder Settings")]
    [Tooltip("The force that the sleep powder is initially thrown at.")]
    [SerializeField] private float throwForce;

    private void Awake() {
        ai = GetComponent<EnemyAI>();
    }


    public override void StartSpecialAttack() {
        ai.animator.SetTrigger("SleepPowderAttack");
    }

    public override void PerformSpecialAttack() {
        SpawnSleepPowder();
    }

    private void SpawnSleepPowder() {
        sleepPowder = Instantiate(sleepPowderPrefab, sleepPowderSpawnTransform);
        sleepPowder.GetComponent<EnemyProjectile>().damage = specialAttackDamage;
        sleepattack_audioSource.PlayOneShot(spawn_sleep_powder_sfx);
    }

    public void ThrowSleepPowder() {
        if (!sleepPowder) return;

        sleepPowder.transform.SetParent(null);
        sleepPowder.transform.rotation = Quaternion.LookRotation((ai.playerTransform.position - sleepPowder.transform.position).normalized);
        sleepPowder.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        sleepPowder.GetComponent<Rigidbody>().AddForce(sleepPowder.transform.forward * throwForce, ForceMode.Impulse);
        sleepPowder.GetComponent<BerryBomb>().enabled = true;
        sleepPowder = null;
        sleepattack_audioSource.PlayOneShot(throw_sleep_powder_sfx);
    }
}
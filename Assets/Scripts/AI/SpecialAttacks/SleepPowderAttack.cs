using UnityEngine;

public class SleepPowderAttack : SpecialAttackBase {
    [Header("Sleep Powder References")]
    [SerializeField] private GameObject sleepPowderPrefab;
    [SerializeField] private Transform sleepPowderSpawnTransform;

    private EnemyAI ai;
    private GameObject sleepPowder;

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
    }

    public void ThrowSleepPowder() {
        if (!sleepPowder) return;

        sleepPowder.transform.SetParent(null);
        sleepPowder.transform.rotation = Quaternion.LookRotation((ai.playerTransform.position - sleepPowder.transform.position).normalized);
        sleepPowder.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        sleepPowder.GetComponent<Rigidbody>().AddForce(sleepPowder.transform.forward * throwForce, ForceMode.Impulse);
        sleepPowder = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyAI {
    [Header("Range Enemy References")]
    [SerializeField] private Transform projectileSpawnTransform;
    [SerializeField] private GameObject projectilePrefab;
    public GameObject loadedProjectile;

    private OffCooldownNode shootCooldownNode;

    [Header("Range Enemy Settings")]
    [Tooltip("The initial force that the projectile of the basic attack is thrown at.")]
    [SerializeField] private float projectileForce;

    [Header("Sounds")]
    [SerializeField] private AudioSource AudioSource_missile;
    [SerializeField] private AudioSource AudioSource_voice;
    [SerializeField] private AudioClip[] missile_charge_sfx;
    //[SerializeField] private AudioClip[] missile_shoot_sfx;    
    [SerializeField] private AudioClip[] missile_travel_sfx;
    [SerializeField] private AudioClip[] missile_travel_voice;
    // --AUDIO-- // 
    private void PlayRandomClip(AudioClip[] clips, AudioSource audioSource) {
        if (clips.Length > 0) {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);
            audioSource.Play();
        }
    }

    public override void ConstructBehaviorTree() {
        shootCooldownNode = new OffCooldownNode(this, basicAttackCooldown);

        root = new Sequence(new List<Node> {  // Does this meet the requirements in order to be aggressive?
            new Selector(new List<Node> {  // Do either of these requirements meet?
                new RangeNode(chasingRange, playerTransform, transform),
                new Inverter(new HealthMinThresholdNode(health, health.maxHealth))
            }),
            new Selector(new List<Node> {  // Decide the aggressive action that fits best.
                new Sequence(new List<Node> {  // Does this meet the requirements to rotate towards player?
                    new IsAttackingNode(this),
                    new RotateTowardsPlayerNode(this, playerTransform, transform)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements to basic attack?
                    shootCooldownNode,
                    new RangeNode(basicAttackRange, playerTransform, transform),
                    new ShootNode(animator, agent, this, shootCooldownNode)
                }),
                new Sequence(new List<Node> {  // Does this meet the requirements to chase?
                    new Inverter(new RangeNode(minimumChaseRange, playerTransform, transform)),
                    new ChaseNode(animator, playerTransform, agent)
                }),
            }),
        });
    }

    public IEnumerator LoadProjectile() {
        while (Time.time < shootCooldownNode.lastTimeUsed + basicAttackCooldown) yield return null;
        //yield return new WaitForSeconds(basicAttackCooldown);
        InstantLoadProjectile();
    }

    private void InstantLoadProjectile() {
        if (loadedProjectile != null) return;
        loadedProjectile = Instantiate(projectilePrefab, projectileSpawnTransform);
        if (AudioSource_missile) PlayRandomClip(missile_charge_sfx, AudioSource_missile);
    }

    public void RangeAttack() {
        if (loadedProjectile == null) InstantLoadProjectile();

        loadedProjectile.transform.SetParent(null);
        // Get the AudioSource component from the instantiated bread and play the spawnbread_sfx on throw
        AudioSource missileAudioSource = loadedProjectile.GetComponent<AudioSource>();
        loadedProjectile.transform.rotation = Quaternion.LookRotation((playerTransform.position - loadedProjectile.transform.position).normalized);
        loadedProjectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        loadedProjectile.GetComponent<Rigidbody>().AddForce(loadedProjectile.transform.forward * projectileForce, ForceMode.Impulse);
        EnemyProjectile enemyProjectile = loadedProjectile.AddComponent<EnemyProjectile>();
        enemyProjectile.damage = basicAttackDamage;
        loadedProjectile = null;
        PlayRandomClip(missile_travel_sfx, missileAudioSource);
        PlayRandomClip(missile_travel_voice, AudioSource_voice);

        while (projectileSpawnTransform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}

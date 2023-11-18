using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyAI {
    [Header("Range Enemy References")]
    [SerializeField] private Transform projectileSpawnTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject loadedProjectile;

    [Header("Range Enemy Settings")]
    [Tooltip("The initial force that the projectile of the basic attack is thrown at.")]
    [SerializeField] private float projectileForce;

    [Header("Sounds")]
    [SerializeField] private AudioSource AudioSource_missile;
    [SerializeField] private AudioClip[] missile_charge_sfx;
    //[SerializeField] private AudioClip[] missile_shoot_sfx;    
    [SerializeField] private AudioClip[] missile_travel_sfx;    

    // --AUDIO-- // 
    private void PlayRandomClip(AudioClip[] clips, AudioSource audioSource)
    {
        if (clips.Length > 0)
        {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);
            audioSource.Play();
        }
    }

    public override void ConstructBehaviorTree() {
        OffCooldownNode shootCooldownNode = new OffCooldownNode(this, basicAttackCooldown);

        root = new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new IsAttackingNode(this),
                new RotateTowardsPlayerNode(this, playerTransform, transform)
            }),
            new Sequence(new List<Node> {
                shootCooldownNode,
                new RangeNode(basicAttackRange, playerTransform, transform),
                new ShootNode(animator, agent, this, shootCooldownNode)
            }),
            new Sequence(new List<Node> {
                new RangeNode(chasingRange, playerTransform, transform),
                new ChaseNode(animator, playerTransform, agent)
            })
        });
    }

    public IEnumerator LoadProjectile() {
        yield return new WaitForSeconds(basicAttackCooldown);
        loadedProjectile = Instantiate(projectilePrefab, projectileSpawnTransform);
        PlayRandomClip(missile_charge_sfx, AudioSource_missile);
    }

    public void RangeAttack() {
        loadedProjectile.transform.parent = null;
        // Get the AudioSource component from the instantiated bread and play the spawnbread_sfx on throw
        AudioSource missileAudioSource = loadedProjectile.GetComponent<AudioSource>();
        loadedProjectile.transform.rotation = Quaternion.LookRotation((playerTransform.position - loadedProjectile.transform.position).normalized);
        loadedProjectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        loadedProjectile.GetComponent<Rigidbody>().AddForce(loadedProjectile.transform.forward * projectileForce, ForceMode.Impulse);
        loadedProjectile = null;
        PlayRandomClip(missile_travel_sfx, missileAudioSource);
    }

    public void SpecialAttack() {
        specialAttack.PerformSpecialAttack();
    }
}

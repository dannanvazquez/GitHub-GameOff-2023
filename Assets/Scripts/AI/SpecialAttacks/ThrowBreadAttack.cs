using System.Collections;
using UnityEngine;

public class ThrowBreadAttack : SpecialAttackBase {
    [Header("Throw Bread References")]
    [SerializeField] private Transform[] breadSpawnPointTransforms;
    [SerializeField] private GameObject breadPrefab;

    private EnemyAI ai;

    [Header("Audiosources & Audioclips")]
    [SerializeField] private AudioSource AudioSource_Breadspawn;
    [SerializeField] private AudioSource AudioSource_rangeattack;
    [SerializeField] private AudioClip[] spawnbread_sfx;
    [SerializeField] private AudioClip[] throwbread_sfx;
    [SerializeField] private AudioClip[] rangeattack_sfx;
    private AudioClip lastspawnbreadClip;
    private AudioClip lastthrownbreadClip;
    
    [Header("Throw Bread Settings")]
    [Tooltip("The force that the bread is initially thrown at.")]
    [SerializeField] private float throwForce;
    [Tooltip("The amount of seconds between each bread spawns.")]
    [SerializeField] private float spawnBreadIntervals;
    [Tooltip("The amount of seconds after bread is spawned before it's thrown.")]
    [SerializeField] private float throwBuffer;

    private void Awake() {
        ai = GetComponent<EnemyAI>();
    }

    // --AUDIO-- // 
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
    private void PlayRandomClipNoLast(AudioClip[] clips, AudioSource audioSource)
    {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);
            audioSource.Play();
    }

    public override void StartSpecialAttack() {
        ai.animator.SetTrigger("SpecialAttack");
    }

    public override void PerformSpecialAttack() {
        PlayRandomClip(rangeattack_sfx, ref lastspawnbreadClip, AudioSource_rangeattack);
        StartCoroutine(SpawnBread());
    }

    private IEnumerator SpawnBread() {
        foreach (Transform t in breadSpawnPointTransforms) {
            GameObject bread = Instantiate(breadPrefab, t.position, t.rotation);
            PlayRandomClip(spawnbread_sfx, ref lastspawnbreadClip, AudioSource_Breadspawn);
            bread.GetComponent<EnemyProjectile>().damage = specialAttackDamage;
            StartCoroutine(ThrowBread(bread));
            yield return new WaitForSeconds(spawnBreadIntervals);
        }
    }

    private IEnumerator ThrowBread(GameObject bread) {
        yield return new WaitForSeconds(throwBuffer);
        // Get the AudioSource component from the instantiated bread and play the spawnbread_sfx on throw
        if (bread) {
            AudioSource breadAudioSource = bread.GetComponent<AudioSource>();
            PlayRandomClipNoLast(throwbread_sfx, breadAudioSource);
            bread.transform.rotation = Quaternion.LookRotation((ai.playerTransform.position - bread.transform.position).normalized);
            bread.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            bread.GetComponent<Rigidbody>().AddForce(bread.transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}

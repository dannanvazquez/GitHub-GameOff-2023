using System.Collections;
using UnityEngine;

public class Ice : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    private EnemyHealth health;
    private EnemyAI ai;

    [HideInInspector] public bool isFrozen;
    private float frozenTime;

    [Header("Sounds")]
    [SerializeField] private AudioSource AudioSource_burning;
    [SerializeField] private AudioClip intro_sfx;  
    [SerializeField] private AudioClip ice_sfx;
    private bool hasPlayedIntro = false;
    
    private void Awake() {
        health = GetComponent<EnemyHealth>();
        ai = GetComponent<EnemyAI>();
    }

    public void SetFrozen(float freezeTime) {
        if (frozenTime < freezeTime) frozenTime = freezeTime;

        if (isFrozen) return;
        isFrozen = true;
        particles.Play();
        ai.animator.StartPlayback();
        ai.agent.isStopped = true;
        if (!hasPlayedIntro)
            {
                AudioSource_burning.PlayOneShot(intro_sfx);
                hasPlayedIntro = true;

                // Schedule the looping sound to start after the intro sound finishes
                Invoke("PlayLoopedSound", intro_sfx.length);
            }
        StartCoroutine(WhileFrozen());

    }

    private IEnumerator WhileFrozen() {
        while (frozenTime > 0) {
            frozenTime -= Time.deltaTime;
            yield return null;
        }

        isFrozen = false;
        particles.Stop();
        AudioSource_burning.Stop();
        ai.animator.StopPlayback();
    }

        private void PlayLoopedSound()
    {
        Debug.Log("loopedsfx");
        // Play the burning sound in a loop
        AudioSource_burning.clip = ice_sfx;
        AudioSource_burning.loop = true;
        AudioSource_burning.Play();
    }
}

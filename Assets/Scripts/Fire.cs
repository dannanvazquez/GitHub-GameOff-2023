using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fire : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    private EnemyHealth health;

    [Header("Settings")]
    [Tooltip("The amount of damage fire does each interval.")]
    [SerializeField] private float fireDamage;
    [Tooltip("The amount of seconds in between each fire tick.")]
    [SerializeField] private float fireInterval;
    [Tooltip("The amount of fire ticks before it extinguishes.")]
    [SerializeField] private int fireTicksAmount;
    
    [Header("Sounds")]
    [SerializeField] private AudioSource AudioSource_burning;
    [SerializeField] private AudioClip intro_sfx;  
    [SerializeField] private AudioClip burning_sfx;
    private bool hasPlayedIntro = false;
    
    private int currentFireTicks;
    private bool isOnFire;

    private void Awake() {
        health = GetComponent<EnemyHealth>();
    }

    public void SetOnFire() {
        currentFireTicks = fireTicksAmount;

        if (isOnFire) return;
        isOnFire = true;
        particles.Play();

        Invoke(nameof(FireDamage), fireInterval);
        if (!hasPlayedIntro)
            {
                AudioSource_burning.PlayOneShot(intro_sfx);
                hasPlayedIntro = true;

                // Schedule the looping sound to start after the intro sound finishes
                Invoke("PlayLoopedSound", intro_sfx.length);
            }
    }

    public void FireDamage() {
        health.TakeDamage(fireDamage);

        currentFireTicks--;
        if (currentFireTicks > 0) {
            Invoke(nameof(FireDamage), fireInterval);
        } else {
            isOnFire = false;
            particles.Stop();
            AudioSource_burning.Stop();
            return;
        }
    }

        private void PlayLoopedSound()
    {
        Debug.Log("loopedsfx");
        // Play the burning sound in a loop
        AudioSource_burning.clip = burning_sfx;
        AudioSource_burning.loop = true;
        AudioSource_burning.Play();
    }
}

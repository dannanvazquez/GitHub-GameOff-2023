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
    }

    public void FireDamage() {
        health.TakeDamage(fireDamage);

        currentFireTicks--;
        if (currentFireTicks > 0) {
            Invoke(nameof(FireDamage), fireInterval);
        } else {
            isOnFire = false;
            particles.Stop();
            return;
        }
    }
}

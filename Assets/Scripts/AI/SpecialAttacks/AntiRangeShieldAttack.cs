using UnityEngine;

public class AntiRangeShieldAttack : SpecialAttackBase {
    [Header("Anti-Range Shield Attack References")]
    [SerializeField] private ParticleSystem shieldParticles;

    [Header("Audio")]   
    [SerializeField] private AudioSource audiosource_shield;
    [SerializeField] private AudioClip shield_broken_sfx;
    [SerializeField] private AudioClip shield_activated_sfx;
    
    private EnemyAI ai;

    [HideInInspector] public bool shieldActive;

    private void Awake() {
        ai = GetComponent<EnemyAI>();
    }

    public override void StartSpecialAttack() {
        if (!shieldActive) {
            PerformSpecialAttack();
        }
        ai.DoneAttacking();
    }

    public override void PerformSpecialAttack() {
        shieldActive = true;
        shieldParticles.Play();
        audiosource_shield.PlayOneShot(shield_activated_sfx);
    }

    public void DisableShield() {
        shieldActive = false;
        shieldParticles.Stop();
        audiosource_shield.PlayOneShot(shield_broken_sfx);

    }
}

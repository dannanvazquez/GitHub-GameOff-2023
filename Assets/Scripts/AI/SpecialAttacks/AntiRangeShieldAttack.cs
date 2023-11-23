using UnityEngine;

public class AntiRangeShieldAttack : SpecialAttackBase {
    [Header("Anti-Range Shield Attack References")]
    [SerializeField] private ParticleSystem shieldParticles;

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
    }

    public void DisableShield() {
        shieldActive = false;
        shieldParticles.Stop();
    }
}

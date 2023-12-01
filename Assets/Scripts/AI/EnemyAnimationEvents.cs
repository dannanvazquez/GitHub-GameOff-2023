using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour {
    public void BasicAttackParticles() => transform.parent.GetComponent<EnemyAI>().BasicAttackParticles();

    public void RangeAttack() => transform.parent.GetComponent<RangeEnemy>().RangeAttack();

    public void DoneAttacking() => transform.parent.GetComponent<EnemyAI>().DoneAttacking();

    public void PerformDuplicateAttack() => transform.parent.GetComponent<DuplicateAttack>().PerformSpecialAttack();

    public void PerformSleepPowderAttack() => transform.parent.GetComponent<SleepPowderAttack>().PerformSpecialAttack();

    public void ThrowSleepPowder() => transform.parent.GetComponent<SleepPowderAttack>().ThrowSleepPowder();

    public void MeleeAttack() {
        if (transform.parent.TryGetComponent(out BBBossEnemy bbBossEnemy)) {
            bbBossEnemy.MeleeAttack();
        } else if (transform.parent.TryGetComponent(out BBMinionEnemy bbMinionEnemy)) {
            bbMinionEnemy.MeleeAttack();
        }
    }

    public void ToggleSword(int toggle) {
        if (transform.parent.TryGetComponent(out BBBossEnemy bbBossEnemy)) {
            bbBossEnemy.ToggleSword(toggle);
        } else if (transform.parent.TryGetComponent(out BBMinionEnemy bbMinionEnemy)) {
            bbMinionEnemy.ToggleSword(toggle);
        }
    }

    public void DeathVisuals() => transform.parent.GetComponent<EnemyHealth>().DeathVisuals();
}

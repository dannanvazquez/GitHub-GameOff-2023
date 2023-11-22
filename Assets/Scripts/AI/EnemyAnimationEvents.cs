using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour {
    public void BasicAttackParticles() => transform.parent.GetComponent<EnemyAI>().BasicAttackParticles();

    public void RangeAttack() => transform.parent.GetComponent<RangeEnemy>().RangeAttack();

    public void DoneAttacking() => transform.parent.GetComponent<EnemyAI>().DoneAttacking();

    public void PerformDuplicateAttack() => transform.parent.GetComponent<DuplicateAttack>().PerformSpecialAttack();

    public void PerformSleepPowderAttack() => transform.parent.GetComponent<SleepPowderAttack>().PerformSpecialAttack();

    public void ThrowSleepPowder() => transform.parent.GetComponent<SleepPowderAttack>().ThrowSleepPowder();

    public void MeleeAttack() => transform.parent.GetComponent<BBBossEnemy>().MeleeAttack();

    public void ToggleSword(int toggle) => transform.parent.GetComponent<BBBossEnemy>().ToggleSword(toggle);
}

using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour {
    public void BasicAttackParticles() => transform.parent.GetComponent<EnemyAI>().BasicAttackParticles();

    public void RangeAttack() => transform.parent.GetComponent<RangeEnemy>().RangeAttack();

    public void DoneBasicAttacking() => transform.parent.GetComponent<EnemyAI>().DoneBasicAttacking();
}

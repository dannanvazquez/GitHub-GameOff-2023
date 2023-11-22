using UnityEngine;
using UnityEngine.AI;

public class SpecialAttackNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private OffCooldownNode offCooldownNode;
    private SpecialAttackBase specialAttackBase;

    public SpecialAttackNode(Animator animator, NavMeshAgent agent, EnemyAI ai, OffCooldownNode offCooldownNode, SpecialAttackBase specialAttackBase) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.offCooldownNode = offCooldownNode;
        this.specialAttackBase = specialAttackBase;
    }

    public override NodeState Evaluate() {
        offCooldownNode.lastTimeUsed = Time.time;

        if (specialAttackBase.specialAttackChancePerc == 1 || Random.value < specialAttackBase.specialAttackChancePerc) {
            specialAttackBase.StartSpecialAttack();
            if (agent.enabled) agent.isStopped = true;
            Debug.Log("Special Attack Node is attacking");
            ai.isAttacking = true;
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

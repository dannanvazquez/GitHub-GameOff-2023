using UnityEngine;
using UnityEngine.AI;

public class SpecialAttackNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private OffCooldownNode offCooldownNode;

    public SpecialAttackNode(Animator animator, NavMeshAgent agent, EnemyAI ai, OffCooldownNode offCooldownNode) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.offCooldownNode = offCooldownNode;
    }

    public override NodeState Evaluate() {
        animator.SetTrigger("SpecialAttack");
        if (agent.enabled) agent.isStopped = true;
        ai.isSpecialAttacking = true;
        offCooldownNode.lastTimeUsed = Time.time;

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

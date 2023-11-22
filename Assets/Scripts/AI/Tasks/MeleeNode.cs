using UnityEngine;
using UnityEngine.AI;

public class MeleeNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private OffCooldownNode offCooldownNode;

    public MeleeNode(Animator animator, NavMeshAgent agent, EnemyAI ai, OffCooldownNode offCooldownNode) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.offCooldownNode = offCooldownNode;
    }

    public override NodeState Evaluate() {
        animator.SetTrigger("Melee");
        agent.isStopped = true;
        Debug.Log("Melee Node is attacking");
        ai.isAttacking = true;
        offCooldownNode.lastTimeUsed = Time.time;

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

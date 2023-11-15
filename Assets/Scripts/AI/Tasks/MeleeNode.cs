using UnityEngine;
using UnityEngine.AI;

public class MeleeNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private PlayerHealth health;

    public MeleeNode(Animator animator, NavMeshAgent agent, EnemyAI ai, PlayerHealth health) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.health = health;
    }

    public override NodeState Evaluate() {
        animator.SetTrigger("Melee");
        agent.isStopped = true;
        ai.isAttacking = true;
        ai.lastTimeAttacked = Time.time;

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

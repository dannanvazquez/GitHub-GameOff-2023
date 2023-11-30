using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private RangeEnemy ai;
    private OffCooldownNode offCooldownNode;

    public ShootNode(Animator animator, NavMeshAgent agent, RangeEnemy ai, OffCooldownNode offCooldownNode) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.offCooldownNode = offCooldownNode;
    }

    public override NodeState Evaluate() {
        if (ai.loadedProjectile == null) {
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }

        animator.SetTrigger("Shoot");
        if (agent.enabled) agent.isStopped = true;
        ai.isAttacking = true;
        offCooldownNode.lastTimeUsed = Time.time;
        ai.StartCoroutine(ai.LoadProjectile());

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

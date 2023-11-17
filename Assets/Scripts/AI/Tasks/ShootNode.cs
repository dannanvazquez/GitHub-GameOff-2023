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
        animator.SetTrigger("Shoot");
        if (agent.enabled) agent.isStopped = true;
        ai.isBasicAttacking = true;
        ai.StartCoroutine(ai.LoadProjectile());
        offCooldownNode.lastTimeUsed = Time.time;

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

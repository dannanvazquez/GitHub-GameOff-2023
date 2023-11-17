using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node {
    private Animator animator;
    private Transform target;
    private NavMeshAgent agent;

    public ChaseNode(Animator animator, Transform target, NavMeshAgent agent) {
        this.animator = animator;
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate() {
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance > 0.2f) {
            animator.SetBool("IsWalking", true);
            if (agent.enabled) {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            _nodeState = NodeState.RUNNING;
        } else {
            animator.SetBool("IsWalking", false);
            if (agent.enabled) agent.isStopped = true;
            _nodeState = NodeState.SUCCESS;
        }

        return _nodeState;
    }
}

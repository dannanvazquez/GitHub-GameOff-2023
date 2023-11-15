using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node {
    private Animator animator;
    private NavMeshAgent agent;

    public ShootNode(Animator animator, NavMeshAgent agent) {
        this.animator = animator;
        this.agent = agent;
    }

    public override NodeState Evaluate() {
        agent.isStopped = true;
        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

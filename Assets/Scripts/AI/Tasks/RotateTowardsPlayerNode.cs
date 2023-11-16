using UnityEngine;

public class RotateTowardsPlayerNode : Node {
    private EnemyAI ai;
    private Transform target;
    private Transform origin;

    public RotateTowardsPlayerNode(EnemyAI ai, Transform target, Transform origin) {
        this.ai = ai;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate() {
        Quaternion direction = Quaternion.LookRotation((target.position - origin.position).normalized);
        direction.x = origin.rotation.x;
        direction.z = origin.rotation.z;
        ai.SetRotation(direction);

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}

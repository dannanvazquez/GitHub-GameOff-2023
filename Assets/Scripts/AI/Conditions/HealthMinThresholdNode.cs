using UnityEngine;

public class HealthMinThresholdNode : Node {
    private EnemyHealth health;
    private float threshold;

    public HealthMinThresholdNode(EnemyHealth health, float threshold) {
        this.health = health;
        this.threshold = threshold;
    }

    public override NodeState Evaluate() {
        _nodeState = health.currentHealth >= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}

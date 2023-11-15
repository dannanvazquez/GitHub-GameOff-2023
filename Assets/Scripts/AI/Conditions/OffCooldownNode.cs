using Unity.VisualScripting;
using UnityEngine;

public class OffCooldownNode : Node {
    private EnemyAI ai;
    private float cooldown;

    public OffCooldownNode(EnemyAI ai, float cooldown) {
        this.ai = ai;
        this.cooldown = cooldown;
    }

    public override NodeState Evaluate() {
        _nodeState = Time.time >= ai.lastTimeAttacked + cooldown ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}

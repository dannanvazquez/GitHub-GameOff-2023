using Unity.VisualScripting;
using UnityEngine;

public class OffCooldownNode : Node {
    private EnemyAI ai;
    private float cooldown;

    public float lastTimeUsed;

    public OffCooldownNode(EnemyAI ai, float cooldown) {
        this.ai = ai;
        this.cooldown = cooldown;
        lastTimeUsed -= cooldown;
    }

    public override NodeState Evaluate() {
        _nodeState = Time.time >= lastTimeUsed + cooldown ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}

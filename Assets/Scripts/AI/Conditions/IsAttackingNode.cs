public class IsAttackingNode : Node {
    private EnemyAI ai;

    public IsAttackingNode(EnemyAI ai) {
        this.ai = ai;
    }

    public override NodeState Evaluate() {
        _nodeState = ai.isBasicAttacking || ai.isSpecialAttacking ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}

using System.Collections.Generic;

public class Sequence : Node {
    protected List<Node> children = new List<Node>();

    public Sequence(List<Node> children) {
        this.children = children;
    }

    public override NodeState Evaluate() {
        bool isAnyChildrenRunning = false;

        foreach (var child in children) {
            switch (child.Evaluate()) {
                case NodeState.RUNNING:
                    isAnyChildrenRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }
        }

        _nodeState = isAnyChildrenRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}

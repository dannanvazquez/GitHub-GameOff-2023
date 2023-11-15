using System.Collections.Generic;

public class Selector : Node {
    protected List<Node> children = new List<Node>();

    public Selector(List<Node> children) {
        this.children = children;
    }

    public override NodeState Evaluate() {
        foreach (var child in children) {
            switch (child.Evaluate()) {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;
            }
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}

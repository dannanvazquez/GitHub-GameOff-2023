using System.Collections.Generic;
using System.Security.Cryptography;

public class Inverter : Node {
    protected Node child;

    public Inverter(Node child) {
        this.child = child;
    }

    public override NodeState Evaluate() {
            switch (child.Evaluate()) {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    break;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.FAILURE;
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.SUCCESS;
                    break;
                default:
                    break;
            }

        return _nodeState;
    }
}

using System.Collections.Generic;

namespace BehaviorTree {
    public class BT_Sequence : BT_Node {
        public BT_Sequence() : base() { }
        public BT_Sequence(List<BT_Node> children) : base(children) { }

        public override NodeState Evaluate() {
            bool anyChildIsRunning = false;

            foreach (BT_Node node in children) {
                switch (node.Evaluate()) {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
using System.Collections.Generic;

namespace BehaviorTree {
    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class BT_Node {
        protected NodeState state;

        public BT_Node parent;
        protected List<BT_Node> children = new();

        private Dictionary<string, object> dataContext = new();

        public BT_Node() {
            parent = null;
        }

        public BT_Node(List<BT_Node> children) {
            foreach (BT_Node child in children) {
                Attach(child);
            }
        }

        // Attaches a node below the current node.
        private void Attach(BT_Node node) {
            node.parent = this;
            children.Add(node);
        }

        // Evaluates the condition and returns its outcome.
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        // Set data to the key provided.
        public void SetData(string key, object value) {
            dataContext[key] = value;
        }

        // Returns data from the key provided. If the key cannot be found, scale up the tree till it can find the key to return. Returns null if not found.
        public object GetData(string key) {
            if (dataContext.TryGetValue(key, out object value)) {
                return value;
            }

            BT_Node node = parent;
            while (node != null) {
                value = node.GetData(key);
                if (value != null) {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        // Clears data of the key provided. If the key cannot be found, scale up the tree till it can find the key to clear. Returns true if done successfully.
        public bool ClearData(string key) {
            if (dataContext.ContainsKey(key)) {
                dataContext.Remove(key);
                return true;
            }

            BT_Node node = parent;
            while (node != null) {
                if (node.ClearData(key)) {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }
}
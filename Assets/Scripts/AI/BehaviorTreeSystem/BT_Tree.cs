using UnityEngine;

namespace BehaviorTree {
    public abstract class BT_Tree : MonoBehaviour {
        private BT_Node root = null;

        [Header("Settings")]
        public float speed;
        public float fovRange;
        public float attackRange;
        public int attackDamage;

        protected void Start() {
            root = SetupTree();
        }

        private void Update() {
            root?.Evaluate();
        }

        protected abstract BT_Node SetupTree();
    }
}
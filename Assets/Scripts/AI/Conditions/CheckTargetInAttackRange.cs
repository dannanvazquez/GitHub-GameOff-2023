using BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CheckTargetInAttackRange : BT_Node {
    private Transform transform;
    private BT_Tree tree;
    //private Animator animator;

    public CheckTargetInAttackRange(Transform _transform) {
        transform = _transform;
        if (!_transform.TryGetComponent(out tree)) {
            Debug.LogError("Could not retrieve Tree.", _transform);
        }
        //animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");
        if (target == null) {
            state = NodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(transform.position, target.position) <= tree.attackRange) {
            //animator.SetBool("Attacking", true);
            //animator.SetBool("Walking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
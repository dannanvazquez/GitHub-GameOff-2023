using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskIdle : BT_Node {
    private Transform transform;

    public TaskIdle(Transform _transform) {
        transform = _transform;
    }

    public override NodeState Evaluate() {
        parent.SetData("target", null);

        transform.GetComponent<NavMeshAgent>().ResetPath();

        state = NodeState.RUNNING;
        return state;
    }
}
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class InstantTurnMovement : MonoBehaviour {
    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void LateUpdate() {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon) {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }
}
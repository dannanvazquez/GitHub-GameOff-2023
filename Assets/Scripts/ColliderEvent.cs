using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderEvent : MonoBehaviour {
    [SerializeField] private UnityEvent<Collider> onTriggerEnterEvent;
    [SerializeField] private UnityEvent<Collider> onTriggerExitEvent;

    private void OnTriggerEnter(Collider other) => onTriggerEnterEvent?.Invoke(other);

    private void OnTriggerExit(Collider other) => onTriggerExitEvent?.Invoke(other);
}

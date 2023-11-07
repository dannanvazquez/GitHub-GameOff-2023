using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform combatLookAt;
    [SerializeField] private Transform playerObject;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() {
        // Rotate player object
        Vector3 directionToLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        orientation.forward = directionToLookAt.normalized;

        playerObject.forward = directionToLookAt.normalized;
    }
}

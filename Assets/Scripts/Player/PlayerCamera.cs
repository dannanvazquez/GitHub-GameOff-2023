using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform combatLookAt;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private GameObject combatCamera;
    //[SerializeField] private GameObject topDownCamera;
    [SerializeField] private CinemachineShake[] cinemachineShakes;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed;
    public CameraStyle currentStyle { get; private set; } = CameraStyle.Basic;

    public enum CameraStyle {
        Basic,
        Combat,
        TopDown
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        if (currentStyle == CameraStyle.Basic && Input.GetButton("Fire2")) {
            SwitchCameraStyle(CameraStyle.Combat);
        } else if (currentStyle == CameraStyle.Combat && !Input.GetButton("Fire2")) {
            SwitchCameraStyle(CameraStyle.Basic);
        } else {
            // Rotate orientation
            Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDirection.normalized;
        }



        // Rotate player object
        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.TopDown) {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDirection != Vector3.zero) {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }

        else if (currentStyle == CameraStyle.Combat) {
            Vector3 directionToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = directionToCombatLookAt.normalized;

            playerObject.forward = directionToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle) {
        thirdPersonCamera.SetActive(false);
        combatCamera.SetActive(false);
        //topDownCamera.SetActive(false);

        GameObject newCamera = null;
        if (newStyle == CameraStyle.Basic) newCamera = thirdPersonCamera;
        if (newStyle == CameraStyle.Combat) newCamera = combatCamera;
        //if (newStyle == CameraStyle.TopDown) newCamera = topDownCamera;

        newCamera.SetActive(true);

        currentStyle = newStyle;
    }

    public void ShakeCamera(float intensity, float time) {
        foreach (var cinemachineShake in cinemachineShakes) {
            if (cinemachineShake.gameObject.activeSelf) {
                cinemachineShake.ShakeCamera(intensity, time);
                return;
            }
        }
    }
}

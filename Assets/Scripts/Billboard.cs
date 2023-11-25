using UnityEngine;

public class Billboard : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private Vector3 rotateOffset = new(0, 180, 0);

    private Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }

    void LateUpdate() {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(rotateOffset);
    }
}

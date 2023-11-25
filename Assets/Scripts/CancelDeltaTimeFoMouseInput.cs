using Cinemachine;
using UnityEngine;

public class CancelDeltaTimeFoMouseInput : MonoBehaviour {
    void OnEnable() => CinemachineCore.GetInputAxis = GetAxisCustom;
    void OnDisable() => CinemachineCore.GetInputAxis = Input.GetAxis;

    float GetAxisCustom(string axisName) {
        var value = Input.GetAxis(axisName);
        if (axisName == "Mouse X" || axisName == "Mouse Y")
            value /= Time.deltaTime;
        return value;
    }
}
using UnityEngine;

public class SleepPowderFog : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The amount of seconds inside till the player falls asleep.")]
    [SerializeField] private float timeTillSleep;
    [Tooltip("The amount of seconds to put the player to sleep for.")]
    [SerializeField] private float sleepTime;

    private float timeInside;
    private bool hasFallenAsleep;

    private void OnTriggerStay(Collider other) {
        if (hasFallenAsleep || !other.CompareTag("Player")) return;

        timeInside += Time.deltaTime;

        if (timeInside >= timeTillSleep) {
            other.transform.parent.GetComponent<PlayerMovement>().Sleep(sleepTime);
            hasFallenAsleep = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) return;

        timeInside = 0;
    }
}

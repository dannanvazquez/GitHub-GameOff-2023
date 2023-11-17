using System.Collections;
using UnityEngine;

public class Ice : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ParticleSystem particles;

    private EnemyHealth health;
    private EnemyAI ai;

    [HideInInspector] public bool isFrozen;
    private float frozenTime;

    private void Awake() {
        health = GetComponent<EnemyHealth>();
        ai = GetComponent<EnemyAI>();
    }

    public void SetFrozen(float freezeTime) {
        if (frozenTime < freezeTime) frozenTime = freezeTime;

        if (isFrozen) return;
        isFrozen = true;
        particles.Play();
        ai.animator.StartPlayback();
        ai.agent.isStopped = true;

        StartCoroutine(WhileFrozen());
    }

    private IEnumerator WhileFrozen() {
        while (frozenTime > 0) {
            frozenTime -= Time.deltaTime;
            yield return null;
        }

        isFrozen = false;
        particles.Stop();
        ai.animator.StopPlayback();
    }
}

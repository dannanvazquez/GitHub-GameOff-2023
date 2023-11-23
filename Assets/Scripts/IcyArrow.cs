using UnityEngine;

public class IcyArrow : BasicArrow {
    [Header("Icy Arrow References")]
    [SerializeField] private GameObject iceParticlePrefab;

    [Header("Icy Arrow Settings")]
    [Tooltip("The amount of seconds the target is frozen for.")]
    [SerializeField] private float iceDuration;

    public override void OnHit(Collider target) {
        Instantiate(iceParticlePrefab, transform.position, Quaternion.identity);

        if (target.transform.TryGetComponent(out Ice ice)) {
            ice.SetFrozen(iceDuration);
        }
    }
}

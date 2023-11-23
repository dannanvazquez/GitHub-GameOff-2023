using UnityEngine;

public class SleepPowder : EnemyProjectile {
    [Header("Sleep Powder References")]
    [SerializeField] private GameObject sleepPowderFogPrefab;

    protected override void OnHit() {
        Instantiate(sleepPowderFogPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
    }
}

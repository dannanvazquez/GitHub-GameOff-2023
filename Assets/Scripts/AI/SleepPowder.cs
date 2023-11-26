using UnityEngine;

public class SleepPowder : EnemyProjectile {
    [Header("Sleep Powder References")]
    [SerializeField] private GameObject sleepPowderFogPrefab;
    [SerializeField] private AudioSource AudioSource;
        [SerializeField] private AudioClip audioClip_loop_sleep_powder_sfx;
    protected override void OnHit() {
        Instantiate(sleepPowderFogPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
        AudioSource.PlayOneShot(audioClip_loop_sleep_powder_sfx);
    }
}

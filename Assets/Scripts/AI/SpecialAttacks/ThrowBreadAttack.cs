using System.Collections;
using UnityEngine;

public class ThrowBreadAttack : SpecialAttackBase {
    [Header("Throw Bread References")]
    [SerializeField] private Transform[] breadSpawnPointTransforms;
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("Throw Bread Settings")]
    [Tooltip("The force that the bread is initially thrown at.")]
    [SerializeField] private float throwForce;
    [Tooltip("The amount of seconds between each bread spawns.")]
    [SerializeField] private float spawnBreadIntervals;
    [Tooltip("The amount of seconds after bread is spawned before it's thrown.")]
    [SerializeField] private float throwBuffer;

    public override void PerformSpecialAttack() {
        StartCoroutine(SpawnBread());
    }

    private IEnumerator SpawnBread() {
        foreach (Transform t in breadSpawnPointTransforms) {
            GameObject bread = Instantiate(breadPrefab, t.position, t.rotation);
            bread.GetComponent<ThrownBread>().damage = specialAttackDamage;
            StartCoroutine(ThrowBread(bread));

            yield return new WaitForSeconds(spawnBreadIntervals);
        }
    }

    private IEnumerator ThrowBread(GameObject bread) {
        yield return new WaitForSeconds(throwBuffer);

        bread.transform.rotation = Quaternion.LookRotation((playerTransform.position - bread.transform.position).normalized);
        bread.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bread.GetComponent<Rigidbody>().AddForce(bread.transform.forward * throwForce, ForceMode.Impulse);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("References")]
    public Animator animator;
    [SerializeField] private PlayerInventoryManager inventoryManager;
    [SerializeField] private Transform arrowHolderTransform;
    [SerializeField] private GameObject arrowVisual;
    public PlayerCamera playerCamera;
    [SerializeField] private ParticleSystem slashParticles;

    private Vector3 shootPosition;

    [Header("Settings")]
    [Tooltip("The force that the arrow is initially shot at.")]
    [SerializeField] private float arrowForce;
    [Tooltip("The amount of damage a melee hit does to an enemy.")]
    public float meleeDamage;
    [Tooltip("The amount of seconds before the player can shoot again.")]
    [SerializeField] private float shootCooldown;
    [Tooltip("The amount of seconds before the player can melee again.")]
    [SerializeField] private float meleeCooldown;

    private bool isAiming;
    private bool isMeleeing;
    [HideInInspector] public bool isAsleep;

    private bool canMelee = true;
    private bool canShoot = true;

    List<Collider> enemiesHit = new();

    private void Update() {
        if (isAsleep) return;

        if (Input.GetButtonDown("Fire2")) {
            StartAiming();
        } else if (canShoot && isAiming && Input.GetButtonDown("Fire1")) {
            Shoot();
        } else if (Input.GetButtonUp("Fire2")) {
            StopAiming();
        }

        if (canMelee && !isMeleeing && !isAiming && (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.V))) {
            Melee();
        }
    }

    private void StartAiming() {
        isAiming = true;
        arrowVisual.SetActive(true);

        animator.SetTrigger("Aim");
        animator.SetBool("IsAiming", true);
    }

    private void Shoot() {
        canShoot = false;

        animator.SetTrigger("Shoot");

        int hittableLayerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100.0f, hittableLayerMask)) {
            shootPosition = hit.point;
        } else {
            shootPosition = Camera.main.transform.position + Camera.main.transform.forward * 100f;
        }

        Invoke(nameof(ResetShoot), shootCooldown);
    }

    public void ShootArrow() {
        GameObject arrow = Instantiate(inventoryManager.CurrentlySelectedItem(), arrowHolderTransform.position, Quaternion.LookRotation((shootPosition - arrowHolderTransform.position).normalized));
        arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * arrowForce, ForceMode.Impulse);
        arrow.GetComponent<BasicArrow>().hitPos = shootPosition;

        inventoryManager.UseAmmo();
    }

    public void StopAiming() {
        isAiming = false;

        animator.SetBool("IsAiming", false);
        arrowVisual.SetActive(false);
    }

    private void Melee() {
        isMeleeing = true;
        canMelee = false;

        animator.SetTrigger("Melee");

        Collider[] currentEnemiesHit = enemiesHit.ToArray();
        foreach (var enemy in currentEnemiesHit) {
            DealMeleeDamage(enemy);
        }

        Invoke(nameof(ResetMelee), meleeCooldown);
    }

    public void AddMeleeTriggerEnter(Collider collider) {
        if (isMeleeing) DealMeleeDamage(collider);

        enemiesHit.Add(collider);
    }

    public void RemoveMeleeTriggerEnter(Collider collider) {
        enemiesHit.Remove(collider);
    }

    private void DealMeleeDamage(Collider collider) {
        // TODO: Figure out why a null collider is being passed in the first place
        if (collider != null && collider.TryGetComponent(out EnemyHealth enemyHealth)) {
            bool isDead = enemyHealth.TakeDamage(meleeDamage);
            if (collider.TryGetComponent(out AntiRangeShieldAttack antiRangeShieldAttack) && antiRangeShieldAttack.shieldActive) {
                antiRangeShieldAttack.DisableShield();
            }

            if (isDead) enemiesHit.Remove(collider);
        }
    }

    public void MeleeParticles() {
        slashParticles.Play();
    }

    public void DoneMeleeing() {
        isMeleeing = false;
    }

    private void ResetMelee() {
        canMelee = true;
    }

    private void ResetShoot() {
        canShoot = true;
    }
}

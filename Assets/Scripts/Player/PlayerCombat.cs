using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("References")]
    public Animator animator;
    [SerializeField] private PlayerInventoryManager inventoryManager;
    [SerializeField] private Transform arrowHolderTransform;
    [SerializeField] private GameObject arrowVisual;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private ParticleSystem slashParticles;

    private Vector3 shootPosition;

    [Header("Settings")]
    [Tooltip("The force that the arrow is initially shot at.")]
    [SerializeField] private float arrowForce;
    [Tooltip("The amount of damage a melee hit does to an enemy.")]
    [SerializeField] private float meleeDamage;
    [Tooltip("The amount of seconds before the player can shoot again.")]
    [SerializeField] private float shootCooldown;
    [Tooltip("The amount of seconds before the player can melee again.")]
    [SerializeField] private float meleeCooldown;

    private bool isAiming;
    private bool isMeleeing;

    private bool canMelee = true;
    private bool canShoot = true;

    private void Update() {
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
        slashParticles.Play();

        Invoke(nameof(ResetMelee), meleeCooldown);
    }

    public void MeleeHit(Collider collider) {
        if (!isMeleeing) return;

        if (collider.transform.root.TryGetComponent(out EnemyHealth enemyHealth)) {
            enemyHealth.TakeDamage(meleeDamage);

            isMeleeing = false;
        }
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

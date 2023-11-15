using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrowPrefab;
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
        if (canShoot) {
            if (!isAiming && Input.GetButton("Fire2")) {
                StartAiming();
            } else if (isAiming && Input.GetButtonUp("Fire2")) {
                Shoot();
            }
        }

        if (canMelee && !isMeleeing && !isAiming && Input.GetButtonDown("Fire1")) {
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
        animator.SetBool("IsAiming", false);

        int hittableLayerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground"));
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100.0f, hittableLayerMask)) {
            shootPosition = hit.point;
        } else {
            shootPosition = Camera.main.transform.forward * 100f;
        }

        Invoke(nameof(ResetShoot), shootCooldown);
    }

    public void ShootArrow() {
        isAiming = false;

        arrowVisual.SetActive(false);

        GameObject arrow = Instantiate(arrowPrefab, arrowHolderTransform.position, Quaternion.LookRotation((shootPosition - arrowHolderTransform.position).normalized));
        arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * arrowForce, ForceMode.Impulse);
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

    void OnDrawGizmos() {
        if (shootPosition != Vector3.zero) {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(arrowHolderTransform.position, shootPosition);
        }
    }
}

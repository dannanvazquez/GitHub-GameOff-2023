using UnityEngine;

public abstract class SpecialAttackBase : MonoBehaviour {
    [Header("Special Attack Settings")]
    [Tooltip("The distance from the player before this enemy attempts to perform a special attack.")]
    public float specialAttackRange;
    [Tooltip("The amount of seconds in between each special attack attempt.")]
    public float specialAttackCooldown;
    [Tooltip("The amount of damage a special attack does to the player.")]
    public float specialAttackDamage;

    public abstract void PerformSpecialAttack();
}

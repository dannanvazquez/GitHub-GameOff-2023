using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour {
    public void ShootArrow() {
        transform.parent.GetComponent<PlayerCombat>().ShootArrow();
    }

    public void DoneMeleeing() {
        transform.parent.GetComponent<PlayerCombat>().DoneMeleeing();
    }
}

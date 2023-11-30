using UnityEngine;

public class Animator_PlayOnTrigger : MonoBehaviour
{
    private bool hasPlayed = false;
    public Animator animator;
    public Collider collider;

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasPlayed)
        {
            PlayAnimation();
            hasPlayed = true;
        }
    }

    void PlayAnimation()
    {
        animator.SetTrigger("Trigger_Fadein");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostshroom : MonoBehaviour
{
    [Tooltip("The amount of force to apply to the colliding rigidbody.")]
    public float yForce = 1.5f;

    private Animator animator;
    private bool hasAnimationPlayed = false;
    [SerializeField] private AudioSource jumper_audiosource;
    [SerializeField] private AudioClip jump_mushroom_sfx;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasAnimationPlayed && collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(new Vector3(0, yForce * 1000, 0));

                // Play the "AnimMushroomActivate" animation
                if (animator != null)
                {
                    animator.SetTrigger("AnimMushroomActivateTrigger");
                    hasAnimationPlayed = true; // Set the flag to true after playing the animation
                    jumper_audiosource.pitch=Random.Range(0.95f, 1.05f);
                    jumper_audiosource.PlayOneShot(jump_mushroom_sfx);


                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Reset the flag when the player exits
            hasAnimationPlayed = false;
        }
    }
}

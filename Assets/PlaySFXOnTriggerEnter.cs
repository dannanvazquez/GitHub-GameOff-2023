using System.Collections;
using System.Collections.Generic;using UnityEngine;
using UnityEngine.Audio;

public class PlaySFXOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enterSFX;
    [SerializeField] private AudioClip exitSFX;
    [SerializeField] private AudioClip portal_amb_sfx;
    [SerializeField] private AudioSource portal_amb_AudioSource;

    [Header("Colliders")]
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider triggerCollider;

    [Header("Mixer")]
    [SerializeField] private AudioMixer myAudioMixer;
    [Range(200,3000)][SerializeField] private float muffled=3000;
    [Range(17000,22000)][SerializeField] private float not_muffled=22000;

    [Range(-80,-10)][SerializeField] private float normal_level=-25;
    [Range(-20,0)][SerializeField] private float high_level=-5;
    private bool isPlayerInside;

    private void Start()
    {
        FindPlayerCollider();
    }

    private void FindPlayerCollider()
    {
        // Find the player GameObject in the scene
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject is found
        if (playerObject != null)
        {
            // Get the Collider component from the player GameObject
            playerCollider = playerObject.GetComponent<Collider>();

            // Check if the Collider component is found
            if (playerCollider == null)
            {
                Debug.LogError("Player Collider not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider && !isPlayerInside)
        {
            isPlayerInside = true;
            PlayEnterSFX();
            myAudioMixer.SetFloat("MUFFLED_SOUND", muffled);
            myAudioMixer.SetFloat("RVB_SFX", high_level);
            myAudioMixer.SetFloat("RVB_VOICE", high_level);
            portal_amb_AudioSource.clip = portal_amb_sfx;
            portal_amb_AudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider && isPlayerInside)
        {
            isPlayerInside = false;
            PlayExitSFX();
            myAudioMixer.SetFloat("MUFFLED_SOUND", not_muffled);
            myAudioMixer.SetFloat("RVB_SFX", normal_level);
            myAudioMixer.SetFloat("RVB_VOICE", normal_level);
            portal_amb_AudioSource.Stop();
        }
    }

    private void PlayEnterSFX()
    {
        if (audioSource != null && enterSFX != null)
        {
            audioSource.clip = enterSFX;
            audioSource.Play();
        }
    }

    private void PlayExitSFX()
    {
        if (audioSource != null && exitSFX != null)
        {
            audioSource.clip = exitSFX;
            audioSource.Play();
        }
    }
}
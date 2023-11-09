using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerSound : MonoBehaviour
{
    [Header("AudioClips")]
    [Tooltip("Sounds for player attacking")]
    [SerializeField] private AudioClip[] player_attack_voice;
    [SerializeField] private AudioClip[] player_attack_sfx;
    [Tooltip("Sounds for player dying")]
    [SerializeField] private AudioClip[] player_death_voice;
    [SerializeField] private AudioClip[] player_death_sfx;
    [Tooltip("Sounds for player jumping")]
    [SerializeField] private AudioClip[] player_jump_voice;
    [SerializeField] private AudioClip[] player_jump_sfx;
    [Tooltip("Sounds for player getting hit")]    
    [SerializeField] private AudioClip[] player_hit_voice;
    [SerializeField] private AudioClip[] player_hit_sfx;

    [Header("Footsteps")] 
    [Tooltip("Walking grass footsteps sounds")]        
    [SerializeField] private AudioClip[] grassWalkingLeftFS_sfx;
    [SerializeField] private AudioClip[] grassWalkingRightFS_sfx;
    [Tooltip("Running grass footsteps sounds")]   
    [SerializeField] private AudioClip[] grassRunningLeftFS_sfx;
    [SerializeField] private AudioClip[] grassRunningRightFS_sfx;

    [SerializeField] private AudioClip[] leftConcreteFS_sfx;
    [SerializeField] private AudioClip[] rightConcreteFS_sfx;

    [SerializeField] private AudioClip[] leftWoodFS_sfx;
    [SerializeField] private AudioClip[] rightWoodFS_sfx;


    [Header("AudioSources")]    
    [SerializeField] private AudioSource AudioSource_Voice;
    [SerializeField] private AudioSource AudioSource_SFX;
    [SerializeField] private AudioSource AudioSource_FS;


    // Prevent the same audioclip to be played twice in a row
    private AudioClip lastAttackVoiceClip;
    private AudioClip lastAttackSFXClip;
    private AudioClip lastDeathVoiceClip;
    private AudioClip lastDeathSFXClip;
    private AudioClip lastJumpVoiceClip;
    private AudioClip lastJumpSFXClip;
    private AudioClip lastHitVoiceClip;
    private AudioClip lastHitSFXClip;
    private int lastFootstepIndex = -1;

    // Alternate between left and right sfx for footsteps + walking/running bool
    private bool playLeftFootstep;
    private bool playRightFootstep;
    private bool isRunning;

    // Ground materials
    enum FSMaterial
    {
    Grass,
    Concrete,
    Wood,
    Empty
    }

    private void PlayRandomClip(AudioClip[] clips, ref AudioClip lastClip, AudioSource audioSource)
    {
        if (clips.Length > 0)
        {
            AudioClip clip;
            do
            {
                clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            } while (clip == lastClip);

            lastClip = clip;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayMeleeAttack()
    {
        PlayRandomClip(player_attack_voice, ref lastAttackVoiceClip, AudioSource_Voice);
        PlayRandomClip(player_attack_sfx, ref lastAttackSFXClip, AudioSource_SFX);
    }

    public void PlayDeath()
    {
        PlayRandomClip(player_death_voice, ref lastDeathVoiceClip, AudioSource_Voice);
        PlayRandomClip(player_death_sfx, ref lastDeathSFXClip, AudioSource_SFX);
    }

    public void PlayJump()
    {
        PlayRandomClip(player_jump_voice, ref lastJumpVoiceClip, AudioSource_Voice);
        PlayRandomClip(player_jump_sfx, ref lastJumpSFXClip, AudioSource_SFX);
    }

    public void PlayHit()
    {
        PlayRandomClip(player_hit_voice, ref lastHitVoiceClip, AudioSource_Voice);
        PlayRandomClip(player_hit_sfx, ref lastHitSFXClip, AudioSource_SFX);
    }


void Update(){
    // Detect player input for running
    isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        // Check if left footstep flag is set and play left footstep
        if (playLeftFootstep)
        {
            // Check if the player is running and call the appropriate method
            if (isRunning)
            {
                PlayRunningFootstep(true);
            }
            else
            {
                PlayWalkingFootstep(true);
            }
            playLeftFootstep = false;
        }

        // Check if right footstep flag is set and play right footstep
        if (playRightFootstep)
        {
            // Check if the player is running and call the appropriate method
            if (isRunning)
            {
                PlayRunningFootstep(false);
            }
            else
            {
                PlayWalkingFootstep(false);
            }
            playRightFootstep = false;
        }
}

    // Animation event callback with no parameters -> functions to put in the animator for playing the appropriate sounds for walking/running footsteps
    void SetLeftFootstepFlag()
    {
        playLeftFootstep = true;
    }
    void SetRightFootstepFlag()
    {
        playRightFootstep = true;
    }
    void SetLeftFootstepFlagRunning()
    {
        playLeftFootstep = true;
    }
    void SetRightFootstepFlagRunning()
    {
        playRightFootstep = true;
    }

private FSMaterial SurfaceSelect()
{
    RaycastHit hit;
    Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
    Material surfaceMaterial;

    if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
    {
        Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
        if (surfaceRenderer)
        {   Debug.Log("Surface Material: " + surfaceRenderer.sharedMaterial.name);
            surfaceMaterial = surfaceRenderer ? surfaceRenderer.sharedMaterial : null;
            if (surfaceMaterial.name.Contains("Grass"))
            {
                return FSMaterial.Grass;
            }
            else if (surfaceMaterial.name.Contains("Concrete"))
            {
                return FSMaterial.Concrete;
            }
            else if (surfaceMaterial.name.Contains("Wood"))
            {
                return FSMaterial.Wood;
            }
        }
    }

    // Return a default material if none of the specified materials are found
    return FSMaterial.Empty;
}

void PlayWalkingFootstep(bool isLeftFoot)
{
    AudioClip clip;
    FSMaterial surface = SurfaceSelect();

    switch (surface)
    {
        case FSMaterial.Grass:
            clip = isLeftFoot ? grassWalkingLeftFS_sfx[RandomExcept(lastFootstepIndex, grassWalkingLeftFS_sfx.Length)]
                              : grassWalkingRightFS_sfx[RandomExcept(lastFootstepIndex, grassWalkingRightFS_sfx.Length)];
            break;
        case FSMaterial.Concrete:
            clip = isLeftFoot ? leftConcreteFS_sfx[RandomExcept(lastFootstepIndex, leftConcreteFS_sfx.Length)]
                              : rightConcreteFS_sfx[RandomExcept(lastFootstepIndex, rightConcreteFS_sfx.Length)];
            break;
        case FSMaterial.Wood:
            clip = isLeftFoot ? leftWoodFS_sfx[RandomExcept(lastFootstepIndex, leftWoodFS_sfx.Length)]
                              : rightWoodFS_sfx[RandomExcept(lastFootstepIndex, rightWoodFS_sfx.Length)];
            break;
        case FSMaterial.Empty:
        default:
            // Default to grass footstep sound if no specific material is detected
            clip = isLeftFoot ? grassWalkingLeftFS_sfx[RandomExcept(lastFootstepIndex, grassWalkingLeftFS_sfx.Length)]
                              : grassWalkingRightFS_sfx[RandomExcept(lastFootstepIndex, grassWalkingRightFS_sfx.Length)];
            break;
    }

    // Update lastFootstepIndex with the index of the current footstep sound
    lastFootstepIndex = isLeftFoot ? Array.IndexOf(grassWalkingLeftFS_sfx, clip) : Array.IndexOf(grassWalkingRightFS_sfx, clip);
    // Play the footstep sound
    AudioSource_FS.clip = clip;
    AudioSource_FS.volume = UnityEngine.Random.Range(0.2f, 0.23f);
    AudioSource_FS.pitch = UnityEngine.Random.Range(1f, 1.2f);
    AudioSource_FS.Play();
}

void PlayRunningFootstep(bool isLeftFoot)
{
    AudioClip clip;
    FSMaterial surface = SurfaceSelect();

    switch (surface)
    {
        case FSMaterial.Grass:
            clip = isLeftFoot ? grassRunningLeftFS_sfx[RandomExcept(lastFootstepIndex, grassRunningLeftFS_sfx.Length)]
                              : grassRunningRightFS_sfx[RandomExcept(lastFootstepIndex, grassRunningRightFS_sfx.Length)];
            break;
        case FSMaterial.Concrete:
            clip = isLeftFoot ? leftConcreteFS_sfx[RandomExcept(lastFootstepIndex, leftConcreteFS_sfx.Length)]
                              : rightConcreteFS_sfx[RandomExcept(lastFootstepIndex, rightConcreteFS_sfx.Length)];
            break;
        case FSMaterial.Wood:
            clip = isLeftFoot ? leftWoodFS_sfx[RandomExcept(lastFootstepIndex, leftWoodFS_sfx.Length)]
                              : rightWoodFS_sfx[RandomExcept(lastFootstepIndex, rightWoodFS_sfx.Length)];
            break;
        case FSMaterial.Empty:
        default:
            // Default to grass footstep sound if no specific material is detected
            clip = isLeftFoot ? grassRunningLeftFS_sfx[RandomExcept(lastFootstepIndex, grassRunningLeftFS_sfx.Length)]
                              : grassRunningRightFS_sfx[RandomExcept(lastFootstepIndex, grassRunningRightFS_sfx.Length)];
            break;
    }

    // Update lastFootstepIndex with the index of the current footstep sound
    lastFootstepIndex = isLeftFoot ? Array.IndexOf(grassRunningLeftFS_sfx, clip) : Array.IndexOf(grassRunningRightFS_sfx, clip);

    // Play the footstep sound
    AudioSource_FS.clip = clip;
    AudioSource_FS.volume = UnityEngine.Random.Range(0.2f, 0.23f);
    AudioSource_FS.pitch = UnityEngine.Random.Range(1f, 1.2f);
    AudioSource_FS.Play();
}


int RandomExcept(int except, int max)
{
    int result;
    do
    {
        result = UnityEngine.Random.Range(0, max);
    } while (result == except);
    return result;
}

}

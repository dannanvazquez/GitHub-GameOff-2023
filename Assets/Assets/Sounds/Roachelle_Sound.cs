using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RoachelleSound : MonoBehaviour
{
    [Header("AudioClips")]
    [Tooltip("Sounds for roachelle melee attacking")]
    [SerializeField] private AudioClip[] roachelle_attack_voice;
    [SerializeField] private AudioClip[] roachelle_attack_sfx;
    [Tooltip("Sounds for roachelle dying")]
    [SerializeField] private AudioClip[] roachelle_death_voice;
    [SerializeField] private AudioClip[] roachelle_death_sfx;
    [Tooltip("Sounds for roachelle getting hit")]    
    [SerializeField] private AudioClip[] roachelle_hit_voice;
    [SerializeField] private AudioClip[] roachelle_hit_sfx;
    [SerializeField] private AudioClip[] roachelle_bigattack_voice;

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
        AudioClip clip = GetRandomClip(clips, lastClip);
        if (clip != null)
        {
            lastClip = clip;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

private AudioClip GetRandomClip(AudioClip[] clips, AudioClip lastClip)
{
    if (clips.Length == 1)
    {
        // If there is only one clip, return it without checking for duplicates
        return clips[0];
    }

    int attemptCount = 0;
    int maxAttempts = clips.Length * 2; // Adjust the maximum attempts as needed

    AudioClip clip;
    do
    {
        clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        attemptCount++;
    } while (clip == lastClip && attemptCount < maxAttempts);

    // If the loop exceeds the maximum attempts, return null
    return (attemptCount >= maxAttempts) ? null : clip;
}
        private void PlayRandomClipNoLast(AudioClip[] clips, AudioSource audioSource)
    {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch=UnityEngine.Random.Range(.95f, 1.05f);
            audioSource.Play();
    }

    public void PlayMeleeAttack()
    {
        PlayRandomClip(roachelle_attack_voice, ref lastAttackVoiceClip, AudioSource_Voice);
        PlayRandomClip(roachelle_attack_sfx, ref lastAttackSFXClip, AudioSource_SFX);
    }

    public void PlayDeath()
    {
        PlayRandomClip(roachelle_death_voice, ref lastDeathVoiceClip, AudioSource_Voice);
        PlayRandomClip(roachelle_death_sfx, ref lastDeathSFXClip, AudioSource_SFX);
    }


    public void PlayHit()
    {
        PlayRandomClip(roachelle_hit_voice, ref lastHitVoiceClip, AudioSource_Voice);
        PlayRandomClip(roachelle_hit_sfx, ref lastHitSFXClip, AudioSource_SFX);
    }

    public void BigAttack()
    {
        PlayRandomClipNoLast(roachelle_bigattack_voice, AudioSource_Voice);
    }

void Update(){
    // Detect roachelle input for running
    isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        // Check if left footstep flag is set and play left footstep
        if (playLeftFootstep)
        {
            // Check if the roachelle is running and call the appropriate method
                PlayWalkingFootstep(true);
                playLeftFootstep = false;
        }

        // Check if right footstep flag is set and play right footstep
        if (playRightFootstep)
        {
            // Check if the roachelle is running and call the appropriate method

                PlayWalkingFootstep(false);
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


private FSMaterial SurfaceSelect()
{
    RaycastHit hit;
    Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
    Material surfaceMaterial;

    if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
    {
        Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
        if (surfaceRenderer)
        {   //Debug.Log("Surface Material: " + surfaceRenderer.sharedMaterial.name);
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
    AudioSource_FS.volume = UnityEngine.Random.Range(0.5f, 0.7f);
    AudioSource_FS.pitch = UnityEngine.Random.Range(.4f, .45f);
    AudioSource_FS.Play();
}


int RandomExcept(int except, int max)
{
    if (max <= 0)
    {
        return -1; // or some default value indicating an error
    }

    int result;
    do
    {
        result = UnityEngine.Random.Range(0, max);
    } while (result == except);
    return result;
}

}

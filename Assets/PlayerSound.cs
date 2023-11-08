using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private AudioClip[] grassFS_sfx;
    [SerializeField] private AudioClip[] concreteFS_sfx;  
    [SerializeField] private AudioClip[] woodFS_sfx;  

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
                clip = clips[Random.Range(0, clips.Length)];
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


private FSMaterial SurfaceSelect()
{
    RaycastHit hit;
    Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
    Material surfaceMaterial;

    if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
    {
        Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
        if (surfaceRenderer)
        {
            surfaceMaterial = surfaceRenderer.sharedMaterial;
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
}

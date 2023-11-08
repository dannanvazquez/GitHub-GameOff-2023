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

    [Header("AudioSources")]    
    [SerializeField] private AudioSource AudioSource_Voice;
    [SerializeField] private AudioSource AudioSource_SFX;

    // Prevent the same audioclip to be played twice in a row
    private AudioClip lastAttackVoiceClip;
    private AudioClip lastAttackSFXClip;
    private AudioClip lastDeathVoiceClip;
    private AudioClip lastDeathSFXClip;
    private AudioClip lastJumpVoiceClip;
    private AudioClip lastJumpSFXClip;
    private AudioClip lastHitVoiceClip;
    private AudioClip lastHitSFXClip;


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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlay : MonoBehaviour
{

    [SerializeField] private AudioSource wood_audioSource;
    [SerializeField] private AudioClip[] wood_sfx; 

    private void PlayRandomClip(AudioClip[] clips, AudioSource audioSource)
    {
            AudioClip clip;
            clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.pitch = UnityEngine.Random.Range(.95f, 1.15f);
            audioSource.Play();
            
        }
    // Start is called before the first frame update
    void Awake()
    {
        PlayRandomClip(wood_sfx, wood_audioSource);
    }

}

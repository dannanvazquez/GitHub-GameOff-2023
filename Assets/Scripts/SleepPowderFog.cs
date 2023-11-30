using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class SleepPowderFog : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The amount of seconds inside till the player falls asleep.")]
    [SerializeField] private float timeTillSleep;
    [Tooltip("The amount of seconds to put the player to sleep for.")]
    [SerializeField] private float sleepTime;

    [Header("Sounds")]
    [Tooltip("Player sleeping sound")]
    [SerializeField] private AudioClip sleeping_sfx;
    [Tooltip("Player in the zone sound")]
    [SerializeField] private AudioClip player_in_zone_sfx;
    [Tooltip("Player waking up, end of sleeptime")]
    [SerializeField] private AudioClip wake_up_sfx;
    [Tooltip("Volume")]
    [SerializeField] private float volume=0.2f;
    [SerializeField] private float volume2=.1f;
    [SerializeField] private float volume3=1f;
    [Tooltip("AudioSource")]
    [SerializeField] private AudioSource AudioSource_player_sleepy;

    [Header("Mixer")]
    [SerializeField] private AudioMixer myAudioMixer;
    [Range(200,3000)][SerializeField] private float muffled=1000;
    [Range(17000,22000)][SerializeField] private float not_muffled=22000;


    private float timeInside;
    private bool hasFallenAsleep;

    private void OnTriggerStay(Collider other) {
        if (hasFallenAsleep || !other.CompareTag("Player") || !other.transform.parent) return;
        AudioSource_player_sleepy.PlayOneShot(player_in_zone_sfx,volume2);
        timeInside += Time.deltaTime;       
        if (timeInside >= timeTillSleep) {
            other.transform.parent.GetComponent<PlayerMovement>().Sleep(sleepTime);
            hasFallenAsleep = true;
            AudioSource_player_sleepy.PlayOneShot(sleeping_sfx,volume3);
        }
        //if(timeInside == sleepTime){
        //    AudioSource_player_sleepy.PlayOneShot(wake_up_sfx);
        //}
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) return;

        timeInside = 0;
    }




}
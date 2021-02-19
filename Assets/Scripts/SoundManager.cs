using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;

    public AudioSource musicSource;

    public AudioClip click;
    public AudioClip hiredCountryMan; 
    public AudioClip hiredWarMan;
    public AudioClip newDay;
    public AudioClip battle;
    public AudioClip win, lose;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}

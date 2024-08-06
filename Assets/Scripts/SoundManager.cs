using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private AudioSource audioSource;
    private static SoundManager instance;

    public static SoundManager GetInstance()
    {
        if (instance == null)
        {
            instance = new SoundManager();
        }
        return instance;
    }

    private SoundManager() 
    {
        //Singleton constructor
    }
    public void Initialize(AudioSource audioSource) 
    {
        this.audioSource = audioSource;
    }

    public void PlayAudio(AudioClip clip)
    {
        if(audioSource.isPlaying) 
        {
            audioSource.Stop();
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}

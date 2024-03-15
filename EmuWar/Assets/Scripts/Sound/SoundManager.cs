using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip buttonClickSound; // Sound for button clicks
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}

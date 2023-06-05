using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip submitRowSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioClip buttonClickSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        if(soundName == "type")
            audioSource.PlayOneShot(typingSound);
        else if(soundName == "submit row")
            audioSource.PlayOneShot(submitRowSound);
        else if(soundName == "lose")
            audioSource.PlayOneShot(loseSound);
        else if(soundName == "win")
            audioSource.PlayOneShot(winSound);
        else if(soundName == "button click")
            audioSource.PlayOneShot(buttonClickSound);
    }

}

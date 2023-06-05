using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject button;
    public AudioClip audioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = button.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}

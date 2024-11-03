using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void PlayOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}

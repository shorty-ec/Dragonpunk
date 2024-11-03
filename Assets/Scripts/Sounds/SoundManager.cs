using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (other.gameObject.tag == "Player")
        {
            if(audioSource.clip != audioClip)
            {
                Debug.Log("2");
                audioSource.clip = audioClip;
                audioSource.Play();
            } 
        }
    }
}

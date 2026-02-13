using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public AudioClip ambiantsound;
    private AudioSource audioSource;

    void Start()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        if (ambiantsound != null)
            audioSource.PlayOneShot(ambiantsound);
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    
    private AudioSource _audioSource;
    public AudioClip[] audioClips;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }
    

    public void PlaySound(int index)
    {
        _audioSource.PlayOneShot(audioClips[index]);
    }
}

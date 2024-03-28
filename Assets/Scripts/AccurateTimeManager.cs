using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


//adapted from https://www.youtube.com/watch?v=gIjajeyjRfE
public class AccurateTimeManager : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audioSource;
    // [SerializeField] private Intervals[] intervals;
    [SerializeField] private OSUParser osuParser;
    public float sampledTime;
    public int msSampleTime;
    private bool _isPlaying;

    private void Update()
    {
        if (_isPlaying)
        {
            sampledTime = ((audioSource.timeSamples / (float)audioSource.clip.frequency));
            sampledTime *= 1000f;
            msSampleTime = audioSource.timeSamples / audioSource.clip.frequency;
        }
    }

    public void OnSongStart()
    {
        _isPlaying = true;
    }
}
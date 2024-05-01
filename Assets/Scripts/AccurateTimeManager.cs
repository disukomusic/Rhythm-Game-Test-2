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

    public bool hasSongStarted = false;

    private void Update()
    {
        if (_isPlaying)
        {
            sampledTime = ((audioSource.timeSamples / (float)audioSource.clip.frequency));
            sampledTime *= 1000f;
            // msSampleTime = audioSource.timeSamples / audioSource.clip.frequency;
            // msSampleTime *= 1000;
            msSampleTime = (int)sampledTime;

            if (sampledTime == 0 && hasSongStarted)
            {
                GameManager.Instance.OnSongEnd();
            }
        }
    }

    public void OnSongStart()
    {
        _isPlaying = true;
        StartCoroutine(SongStartCoroutine());
    }

    IEnumerator SongStartCoroutine()
    {
        yield return new WaitForSeconds(1f);
        hasSongStarted = true;
        yield return null;
        StopCoroutine(SongStartCoroutine());

    }
}
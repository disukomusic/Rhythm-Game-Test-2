using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class SongParser : MonoBehaviour
{
    public TextAsset songChart;
    public AudioSource musicSource;
    public AudioSource realMusicSource;
    
    public string songName;
    public string artistName;
    public float bpm;
    public string songPath;

    public TMP_Text trackTitleText;
    public TMP_Text artistText;
    public TMP_Text bpmText;
    
    public string[] lines;
    public List<Beat> beats;
    
    public int currentBeat;
    
    int _beatIndex = 0;
    public bool isPlaying = false;
    public float trackDelay;

    private void Start()
    {
        musicSource.Stop();
        
        lines = songChart.text.Split('\n');
        songName = lines[0];
        artistName = lines[1];
        bpm = float.Parse(lines[2]);
        songPath = lines[3];

        musicSource.clip = Resources.Load<AudioClip>(songPath.Trim());
        realMusicSource.clip = Resources.Load<AudioClip>(songPath.Trim());
        
        trackTitleText.text = songName;
        artistText.text = artistName;
        bpmText.text = bpm.ToString();
        
        for (var i = 4; i < lines.Length ; i++) 
        {
            var line = lines[i];
            
            if (line.Contains("W"))
            {
                string waitLengthString = line.Substring(1); // Remove the "W"
                int waitLength = int.Parse(waitLengthString);

                for (var count = 0; count < waitLength; count++)
                {
                    beats.Add(new Beat("P", _beatIndex));
                    _beatIndex++;
                }
            }
            else
            {
                beats.Add(new Beat(line, _beatIndex));
                _beatIndex++;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            musicSource.Play();
            StartCoroutine(PlayWithDelay());
            //stupid way to make it skip the metadata and get to the notes
            currentBeat = 1;
            isPlaying = true; 
        }
    }

    IEnumerator PlayWithDelay()
    {
        yield return new WaitForSeconds(trackDelay);
        realMusicSource.Play();
        
    }
    public void NextBeat()
    {
        if (currentBeat != beats.Count)
        {
            //beats[currentBeat].TriggerEvent();
        
            currentBeat++;
        }
        else
        {
            OnSongEnd();
        }
    }
    void OnSongEnd()
    {
        musicSource.Stop();
    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HDyar.OSUImporter;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class OSUParser : MonoBehaviour
{
    public PlayerSettings playerSettings;
    [SerializeField] private OSUBeatmap songChart;
    [SerializeField] private AccurateTimeManager accurateTimeManager;
    
    [SerializeField] private AudioSource musicSource;
    
    [SerializeField] private TMP_Text trackTitleText;
    [SerializeField] private TMP_Text artistText;
    
    public string songName;
    public string artistName;
    public float bpm = 110;
    

    [SerializeField] private OSUHitObject[] hitObjects;
    [SerializeField] private int _hitObjectIndex;

    public OSUHitObject currentObject;

    public NoteSpawner.NoteType noteType;

    int GetRealLaneNumber(int osuNumber)
    {
        return Mathf.FloorToInt(osuNumber / 64f);
    }

    private void Start()
    {
        musicSource.Stop();
        songChart = playerSettings.SelectedSong;
        
        //get background image
        // if (songChart.Events[0] != null)
        // {
        //     var filename = ((BackgroundEvent)songChart.Events[0]).Filename;
        //     var fullpath = AssetDatabase.GetAssetPath(songChart);
        //     var dir = Path.GetDirectoryName(fullpath)+"\\";
        //     GameManager.Instance.backgroundImage.sprite = Resources.Load<Sprite>(dir+filename);
        // }
        
        hitObjects = songChart.HitObjects;
        songName = songChart.Metadata.Title;
        artistName = songChart.Metadata.Artist;
        musicSource.clip = songChart.General.Clip;
        
        Debug.Log("loaded osu map " + songName + " by " + artistName);
        
        trackTitleText.text = songName;
        artistText.text = artistName;
        
        _hitObjectIndex = 0;
    }

    void Update()
    {
        if (accurateTimeManager.sampledTime + GameManager.Instance.notePreDelay >= hitObjects[_hitObjectIndex].Time &&
            GameManager.Instance.isPlaying && _hitObjectIndex < hitObjects.Length - 1) 
        {
            if (hitObjects[_hitObjectIndex].IsManiaHoldNote)
            {
                noteType = NoteSpawner.NoteType.Hold;
            }
            else
            {
                noteType = NoteSpawner.NoteType.Normal;
            }
            
            NoteSpawner.Instance.SpawnNote(GetRealLaneNumber(hitObjects[_hitObjectIndex].X), noteType,hitObjects[_hitObjectIndex].Time, hitObjects[_hitObjectIndex].EndTime );
            currentObject = hitObjects[_hitObjectIndex];
            _hitObjectIndex++;
        }
    }

    public void OnSongStart()
    {
        musicSource.Play();
    }
}

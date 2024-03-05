using System;
using System.Collections.Generic;
using System.IO;
using HDyar.OSUImporter;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;


public class SongList : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public UpdateSongInfoUI GUI;
    public List<SongInfo> songList = new List<SongInfo>();
    public string[] osuFiles;
    

    public SongInfo currentTrack;
    public int currentTrackIndex;
    

    void Start()
    {
        LoadSongs();
        
        currentTrackIndex = 0;
    }

    private void Update()
    {
        
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                currentTrackIndex++;
                
                if (currentTrackIndex > songList.Count - 1)
                {
                    currentTrackIndex = 0;
                }
                
                UpdateSongListIndex();
            }
        
        
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentTrackIndex--;
                
                if (currentTrackIndex < 0)
                {
                    currentTrackIndex = songList.Count - 1;
                }
                
                UpdateSongListIndex();
            }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    void UpdateSongListIndex()
    {
        currentTrack = songList[currentTrackIndex];
        playerSettings.SetSelectedSong(currentTrack.osuBeatmap); // Pass the OSUBeatmap to PlayerSettings
        GUI.UpdateSongInfo();
    }
    void LoadSongs()
    {
        osuFiles = Directory.GetFiles("Assets/Resources/maps", "*.osu", SearchOption.AllDirectories);
        
        foreach (string _osuFilePath in osuFiles)
        {
            string _resourcePath = Path.ChangeExtension(_osuFilePath.Substring(_osuFilePath.IndexOf("Resources") + "Resources".Length + 1), null);
            _resourcePath = _resourcePath.Replace("\\", "/");
            OSUBeatmap _osuBeatmap = Resources.Load<OSUBeatmap>(_resourcePath);
            if (_osuBeatmap == null)
            {
                Debug.Log("Failed to load OSUBeatmap at path: " + _resourcePath);
                continue;
            }
            SongInfo _songInfo = ConvertToSongInfo(_osuBeatmap);
            _songInfo.osuBeatmap = _osuBeatmap; 
            songList.Add(_songInfo);
        }
        
        GUI.UpdateSongInfo();

    }
    
    SongInfo ConvertToSongInfo(OSUBeatmap osuBeatmap)
    {
        string _artist = osuBeatmap?.Metadata?.Artist ?? "Unknown Artist";
        string _title = osuBeatmap?.Metadata?.Title ?? "Unknown Title";
        string _mapper = osuBeatmap?.Metadata?.Creator ?? "Unknown Mapper";
        string _difficulty = osuBeatmap?.Metadata?.Version ?? "Unknown Difficulty";
    
        SongInfo _songInfo = new SongInfo(_artist, _title, _mapper, _difficulty);

        return _songInfo;
    }

}
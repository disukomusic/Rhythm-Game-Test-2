using System;
using System.Collections;
using System.Collections.Generic;
using HDyar.OSUImporter;
using UnityEngine;
[System.Serializable]
public class SongInfo
{
    public string artist;
    public string title;
    public string mapper;
    public string difficulty;
    public OSUBeatmap osuBeatmap; // New field to store the OSUBeatmap


    public SongInfo(string artist, string title, string mapper, string difficulty)
    {
        this.artist = artist;
        this.title = title;
        this.mapper = mapper;
        this.difficulty = difficulty;
    }
}

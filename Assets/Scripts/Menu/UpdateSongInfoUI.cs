using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateSongInfoUI : MonoBehaviour
{
    [SerializeField] private SongList songList;

    [SerializeField] private TMP_Text SongTitleText;
    [SerializeField] private TMP_Text SongArtistText;


    public void UpdateSongInfo()
    {
        SongTitleText.text = songList.currentTrack.title;
        SongArtistText.text = songList.currentTrack.artist;
    }
}

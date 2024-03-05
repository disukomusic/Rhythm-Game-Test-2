using HDyar.OSUImporter;
using UnityEngine;


[CreateAssetMenu(menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField]
    private OSUBeatmap _selectedSong;

    public OSUBeatmap SelectedSong => _selectedSong;

    public void SetSelectedSong(OSUBeatmap song)
    {
        _selectedSong = song;
    }   
}

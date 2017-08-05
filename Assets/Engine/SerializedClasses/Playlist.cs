using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Playlist
{
    public enum PlaylistType
    {
        REPEAT, STRAIGHTTHROUGH, SHUFFLE
    }

    public string playlistName;
    public List<MusicFile> playlist;
    public PlaylistType loopType;

    public AudioClip RollMusic()
    {
        playlist.Sort((s1, s2) => s1.weight.CompareTo(s2.weight));
        string location = "";
        int maxWeight = 0;

        foreach (MusicFile mf in playlist)
            maxWeight += mf.weight;

        int roll = Random.Range(0, maxWeight);
        foreach (MusicFile mf in playlist)
        {
            location = mf.path;
            roll -= mf.weight;
            if (roll < 0) break;
        }
        AudioClip rolledClip = Resources.Load<AudioClip>("Resources/" + location);
        return rolledClip;
    }
}
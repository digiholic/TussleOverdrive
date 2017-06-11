using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public enum AudioType
    {
        Music,
        Sound,
        Voice
    }
    private Settings game_settings;

    void Start()
    {
        game_settings = GetComponent<Settings>();
    }

    public void PlayAudio(AudioSource source, AudioClip clip, AudioType type)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            if (type == AudioType.Music)
                source.volume = (game_settings.music_volume / 100);
            else if (type == AudioType.Sound)
                source.volume = (game_settings.sfx_volume / 100);
            source.Play();
        }
    }
}

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
        playlist.Sort((s1,s2) => s1.weight.CompareTo(s2.weight));
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

[System.Serializable]
public struct MusicFile
{
    public string name; //The name the song is displayed as
    public string path; //The location in the resources folder the song is
    public int weight; //How often the song is "rolled"
}
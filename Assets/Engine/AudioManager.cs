using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{

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
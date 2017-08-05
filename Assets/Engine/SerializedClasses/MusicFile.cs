using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicFile
{
    public string name; //The name the song is displayed as
    public string path; //The location in the resources folder the song is
    public int weight; //How often the song is "rolled"
}


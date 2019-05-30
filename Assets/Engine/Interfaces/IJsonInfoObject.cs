using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// An interface for objects that write to JSON and can be read in from a JSON textAsset
/// </summary>
public interface IJsonInfoObject
{
    FileInfo Save(string path);
    //TODO load from JSON string
    //TODO load from file path
    void LoadFromTextAsset();
}

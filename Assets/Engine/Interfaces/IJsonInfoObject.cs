using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IJsonInfoObject
{
    FileInfo Save(string path);
    void LoadFromTextAsset();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AudioSerializer : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] private string FilePath;

    public List<string> clipNames = new List<string>();
    public List<AudioClip> clipData = new List<AudioClip>();

    public int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo sfxDir = new DirectoryInfo(FilePath);

        foreach (FileInfo sfx in sfxDir.EnumerateFiles())
        {
            if (sfx.Name.EndsWith(".ptsfx"))
            {
                clipNames.Add(sfx.Name);
                Byte[] bytes = File.ReadAllBytes(sfx.FullName);
                
                clipData.Add(CreateAudioClip(Convert.ToBase64String(bytes),sfx.Name));
                //File.WriteAllBytes(Path.Combine(sfxDir.FullName,sfx.Name+".ptsfx"),bytes);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(String.Format("Now playing {0}", clipNames[index]));
            audioSource.PlayOneShot(clipData[index]);
            index = (index + 1) % clipNames.Count;
        }
    }

    public string WavToBase64(string wavFile)
    {
        Byte[] bytes = File.ReadAllBytes(wavFile);
        return Convert.ToBase64String(bytes);
    }

    public AudioClip CreateAudioClip(String s,String name)
    {
        //string s = Resources.Load<TextAsset>("Sounds/audioClipName").text;

        byte[] bytes = System.Convert.FromBase64String(s);
        float[] f = ConvertByteToFloat(bytes);

        AudioClip audioClip = AudioClip.Create(name, f.Length, 2, getSampleRateFromByteArray(bytes),false);
        
        audioClip.SetData(f, 0);
        return audioClip;

    }

    private int getSampleRateFromByteArray(byte[] array){
        return BitConverter.ToInt32(array,24);
    }

    private float[] ConvertByteToFloat(byte[] array)
    {
        Debug.Log(String.Format("Sample Rate: {0}",BitConverter.ToInt32(array,24)));

        float[] floatArr = new float[array.Length / 2];

        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = ((float)BitConverter.ToInt16(array, i * 2)) / 32768.0f;
        }

        return floatArr;
    }
}

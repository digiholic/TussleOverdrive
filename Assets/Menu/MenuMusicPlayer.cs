using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicPlayer : MonoBehaviour
{
    public static MenuMusicPlayer player;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null){
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            player = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void play(){
        audioSource.Play();
    }

    public void stop(bool fade=true){
        if (fade){
            StartCoroutine(FadeAudio(2));
        } else {
            audioSource.Stop();
        }
    }
    private IEnumerator FadeAudio(int fadeTime){
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
        Destroy(gameObject,2);
    }
}

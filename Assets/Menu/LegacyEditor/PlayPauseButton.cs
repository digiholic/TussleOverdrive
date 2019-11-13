using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPauseButton : MonoBehaviour
{
    [SerializeField] private bool isPlayButton;

    void Update()
    {
        if (LegacyEditorData.instance.isPlaying){
            //if (isPlayButton){
            //    setInvisible();
            //} else {
            //    setVisible();
            //}
        }    
    }

    public void SetVisibility(bool isVisible){
        NGUITools.SetActive(gameObject,isVisible);
    }

    //This class has some shortcut methods that don't take arguments just to make the Unity Editor a bit cleaner
    public void setVisible(){
        SetVisibility(true);
    }

    public void setInvisible(){
        SetVisibility(false);
    }
}

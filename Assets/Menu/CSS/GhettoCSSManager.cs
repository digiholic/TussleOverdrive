using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhettoCSSManager : MonoBehaviour
{
    
    public static GhettoCSSManager manager;
    
    private bool ready;

    [SerializeField] private List<GhettoCSSRig> playerPanels;

    [SerializeField] private GameObject readyBanner;
    void Awake()
    {
        manager = this;
    }
    
    void Update()
    {
        //NGUITools.SetActive(readyBanner,ready);
        readyBanner.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("TopMenu");
        }
        
    }
    public void CheckReady(){
        //We are ready unless:
        //   1. None of the panels are active
        //   2. Any of the panels are active but not confirmed
        bool allDisabled = true;
        bool noneWaiting = true;
        foreach(GhettoCSSRig rig in playerPanels){
            if (rig.isActive) allDisabled = false; //If it's active, then they aren't all disabled
            if (rig.isActive && !rig.isConfirmed) noneWaiting = false; //If it's active and hasn't been confirmed, then someone's still waiting
        }

        ready = (noneWaiting && !allDisabled);
    }
    public void CheckStart(){
        if (ready){
            MenuMusicPlayer.player?.stop(true);
            SceneManager.LoadSceneAsync("stage_TestStage", LoadSceneMode.Single);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionRig : MonoBehaviour {

    public static SelectionRig selection_rig;

    public SelectionPanel[] panels = new SelectionPanel[4];

	void Start()
    {
        selection_rig = this;
    }

    public void CheckStart()
    {
        foreach (SelectionPanel panel in panels)
        {
            if (panel.active == true && panel.confirmed == false)
            {
                return;
            }
        }
        SceneManager.LoadScene("SSS",LoadSceneMode.Single);
    }
}

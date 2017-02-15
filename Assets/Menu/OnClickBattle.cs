using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickBattle : ButtonOnClick {

    public override void OnClick()
    {
        SceneManager.LoadScene("test", LoadSceneMode.Single);
    }
}
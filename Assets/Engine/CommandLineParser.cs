using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandLineParser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs ();
        for (int i = 0; i < args.Length; i++) {
            //If this is an editor command, go straight there
            if (args [i] == "-editor") {
                SceneManager.LoadScene("LegacyEditor", LoadSceneMode.Single);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    public int playerNum;
    public FighterResults results;

    [SerializeField] private TextMeshProUGUI nameLabel;

    void Start()
    {
        ResultsScreen resultsScreen = GameObject.FindObjectOfType<ResultsScreen>();
        bool valid = false;
        if (resultsScreen != null){
            if (resultsScreen.results.Count >= (playerNum+1)){
                results = resultsScreen.results[playerNum];
                if (results.fighterName != ""){
                    nameLabel.text = results.fighterName;
                    valid = true;
                }
            }
        }
        if (!valid){
            gameObject.SetActive(false);
        }
    }
}

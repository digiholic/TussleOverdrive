using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    public List<FighterResults> results = new List<FighterResults>();

    public void AddFighterResult(FighterResults result){
        results.Add(result);
    }
}

[System.Serializable]
public class FighterResults{
    public string fighterName;
    public int stocks = 0;
    public int score = 0;
    public int falls = 0;
    public int selfDestructs = 0;

    public Dictionary<int,int> killsAgainst = new Dictionary<int, int>{
        {0,0},
        {1,0},
        {2,0},
        {3,0}
    };
    public Dictionary<int,int> deathsAgainst = new Dictionary<int, int>{
        {0,0},
        {1,0},
        {2,0},
        {3,0}
    };

    public FighterResults(FighterInfo info){
        fighterName = info.displayName;
    }
}
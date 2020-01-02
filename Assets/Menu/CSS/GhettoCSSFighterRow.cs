using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoCSSFighterRow : MonoBehaviour
{
    public FighterInfo fighter;
    public bool isChosen;
    public bool isHidden;

    UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChosen) label.alpha = 1.0f;
        else if (isHidden) label.alpha = 0.0f;
        else label.alpha = 0.5f;   
    }
}

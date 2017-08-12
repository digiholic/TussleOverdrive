using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStockCount : MonoBehaviour {
    public string varName = "Stock";
    public int varCount = 3;

    public int maxValue = 99;
    public int minValue = 1;
    public int increment = 1;
    public bool allowInfinity = true;

    private UILabel label;
    private bool infinity = false;
	// Use this for initialization
	void Start () {
        label = GetComponentInChildren<UILabel>();
        UpdateLoader();
    }
	
	// Update is called once per frame
	void Update () {
        if (infinity) label.text = varName + ": " + "Infinite";
        else label.text = varName + ": " + varCount.ToString();
	}

    void IncrementValue()
    {
        if (infinity)
        {
            varCount = minValue; //Going up from infinity takes us to min
            infinity = false;
        }
        else
        {
            if (varCount == maxValue)
            {
                if (allowInfinity) infinity = true;
                else varCount = minValue;
            }
            else varCount += increment;
        }
        UpdateLoader();
    }

    void DecrementValue()
    {
        if (infinity)
        {
            varCount = maxValue; //Going down from infinity takes us to max
            infinity = false;
        }
        else
        {
            if (varCount == minValue)
            {
                if (allowInfinity) infinity = true;
                else varCount = maxValue;
            }
            else varCount -= increment;
        }
        UpdateLoader();
    }

    void UpdateLoader()
    {
        if (varName == "Stock")
        {
            BattleLoader.current_loader.stockCount = varCount;
            BattleLoader.current_loader.stockInfinity = infinity;
        }
        if (varName == "Time")
        {
            BattleLoader.current_loader.timeCount = varCount;
            BattleLoader.current_loader.timeInfinity = infinity;
        }
    }
}

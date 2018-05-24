using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsRowRig : MonoBehaviour {
    public List<Setting> settingsToAdd;

    public GameObject slider_setting_prefab;
    public GameObject choice_setting_prefab;

    [SerializeField]
    private SettingsPanel parent;

    //These are used to make sure that all of the buttons are able to navigate to the next ones properly
    private MenuButtonNavigator firstChild = null;
    private MenuButtonNavigator previousChild = null;
    private int settingCount = 0;
	// Use this for initialization
	void Start () {
	    foreach (Setting set in settingsToAdd)
        {
            if (set.options.Count == 0)
            {
                //Debug.Log("Number Setting: " + set.settingName);
                InstantiateNumberRow(set);
            } else
            {
                //Debug.Log("Choice Setting: " + set.settingName);
                //InstantiateChoiceRow(set);
            }
        }
        //Once the loop is over, we need to make sure that the options wrap
        previousChild.Down = firstChild;
        firstChild.Up = previousChild;
        parent.starting_selection = firstChild;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InstantiateNumberRow(Setting set)
    {
        GameObject go = NGUITools.AddChild(gameObject, slider_setting_prefab);
        go.transform.localPosition = new Vector3(0, -80 * settingCount, 0);
        MenuButtonNavigator nav = go.GetComponent<MenuButtonNavigator>();
        SettingsNumberText snt = go.GetComponent<SettingsNumberText>();

        //Set the navigation options
        //If the first child hasn't been set, set it (we'll use it later to make the last one loop around)
        if (firstChild == null)
        {
            firstChild = nav;
            previousChild = nav;
        }
        else
        {
            nav.Up = previousChild;
            previousChild.Down = nav;
            previousChild = nav;
        }

        //Set the text
        snt.var_name = set.settingName;
        snt.display_text = set.displayName;
        snt.value = int.Parse(set.defaultValue);
        if (set.isPercent)
        {
            snt.percentage = true;
            snt.after_text = "%";
        } else
        {
            snt.percentage = false;
            snt.after_text = "";
        }
        snt.min_value = (int)set.minVal;
        snt.max_value = (int)set.maxVal;
        snt.increment = (int)set.increment;

        settingCount++;
    }

    void InstantiateChoiceRow (Setting set)
    {
        GameObject go = NGUITools.AddChild(gameObject, slider_setting_prefab);
        MenuButtonNavigator nav = go.GetComponent<MenuButtonNavigator>();
        SettingsChoiceText sct = go.GetComponent<SettingsChoiceText>();

        //Set the navigation options
        //If the first child hasn't been set, set it (we'll use it later to make the last one loop around)
        if (firstChild == null)
        {
            firstChild = nav;
            previousChild = nav;
        }
        else
        {
            nav.Up = previousChild;
            previousChild.Down = nav;
        }

        //Set the text 

        settingCount++;
    }
}

[System.Serializable]
public struct Setting
{
    public string settingName;
    public string displayName;
    public string defaultValue;
    public bool isPercent;
    public float minVal;
    public float maxVal;
    public float increment;
    public List<string> options;
}
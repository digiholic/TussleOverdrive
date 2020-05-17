using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AutocompletePopulator : MonoBehaviour
{
    public StringListCallback stringListSource;

    private AutoCompleteComboBox comboBox;
    private int cachedListSize;

    private void Awake()
    {
        comboBox = GetComponent<AutoCompleteComboBox>();
    }

    void Update()
    {
        List<string> data = stringListSource.Invoke();
        if (data.Count != cachedListSize)
        {
            comboBox.AvailableOptions = data;
            cachedListSize = data.Count;
            comboBox.RebuildPanel();
        }
    }
}

[System.Serializable]
public class StringListCallback : SerializableCallback<List<string>> { }

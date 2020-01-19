using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResultsDataLine : MonoBehaviour
{
    [SerializeField] private UILabel label;
    [SerializeField] private ResultsPanel parentPanel;
    [SerializeField] private string prefix;
    [SerializeField] private string fieldName;
    // Start is called before the first frame update
    void Start()
    {
        if (label == null) label = GetComponent<UILabel>();
        if (parentPanel == null) parentPanel = GetComponentInParent<ResultsPanel>();
        SetText(fieldName);
    }

    public void SetText(string field){
        if (parentPanel.results == null){
            label.text = "";
            return;
        } else {
            object value = parentPanel.results.GetType().GetField(field).GetValue(parentPanel.results);
            label.text = prefix+": "+value.ToString();
        }
    }
}

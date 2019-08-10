using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupErrorLabel : MonoBehaviour
{
    public float fadeTime = 2;
    private UILabel label;
    private float timeLerped = 0;
    private string errorMessage = "";

    private void Awake()
    {
        label = GetComponent<UILabel>();
    }

    private void Update()
    {
        if (label.alpha != 0)
        {
            timeLerped += Time.deltaTime;
            label.alpha = Mathf.Lerp(1.0f, 0.0f, timeLerped / fadeTime);
        }
    }

    public void SetErrorText(string text)
    {
        errorMessage = text;
    }

    public void DisplayError(string errorText){
        SetErrorText(errorText);
        DisplayError();
    }
    public void DisplayError()
    {
        label.text = errorMessage;
        label.alpha = 1.0f;
        timeLerped = 0;
    }
}

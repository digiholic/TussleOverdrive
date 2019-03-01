using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupErrorLabel : MonoBehaviour
{
    public float fadeTime = 2;
    private UILabel label;
    private float timeLerped = 0;
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

    public void DisplayError(string text="")
    {
        if (text != "")
        {
            label.text = text;
        }
        label.alpha = 1.0f;
        timeLerped = 0;
    }
}

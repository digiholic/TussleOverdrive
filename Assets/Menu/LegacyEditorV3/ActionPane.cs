using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionPane : MonoBehaviour
{
    public DynamicAction action;

    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private MonoActionFile actionFile;

    private Button button;
    private Image buttonImage;
    private Color defaultColor;
    private Color selectedColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonText.text = action.name;
        defaultColor = button.colors.normalColor;
        selectedColor = button.colors.pressedColor;
    }

    private void Update()
    {
        if (actionFile.currentAction == action)
        {
            buttonImage.color = selectedColor;
        } else
        {
            buttonImage.color = defaultColor;
        }
    }
}

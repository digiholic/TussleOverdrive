using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LESelectedAction: MonoBehaviour
{
    public static LESelectedAction instance;

    public DynamicAction CurrentAction;

    [SerializeField] private string actionName;
    [SerializeField] private string length;
    [SerializeField] private string animationName;
    [SerializeField] private string exitAction;

    private void OnEnable()
    {
        instance = this;
    }

    public string ActionName { get => actionName; set
        {
            actionName = value;
            CurrentAction.name = value;
        }
    }
    public string Length { get => length; set
        {
            length = value;
            CurrentAction.length = int.Parse(value);
        }
    }
    public string AnimationName { get => animationName; set
        {
            animationName = value;
            CurrentAction.animationName = value;
        }
    }
    public string ExitAction { get => exitAction; set
        {
            exitAction = value;
            CurrentAction.exit_action = value;
        }
    }

    public void SelectAction(DynamicAction newAction)
    {
        CurrentAction = newAction;
        if (newAction == null)
        {
            actionName = "";
            length = "";
            animationName = "";
            exitAction = "";
        } else
        {
            actionName = newAction.name;
            length = newAction.length.ToString();
            animationName = newAction.animationName;
            exitAction = newAction.exit_action;
        }
    }

    public List<SubactionData> GetSubactions(string key)
    {
        return CurrentAction?.subactionCategories.GetIfKeyExists(key);
    }
}

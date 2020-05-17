using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoActionFile : MonoBehaviour
{
    [SerializeField] private List<DynamicAction> actions;

    public List<DynamicAction> Animations { get => actions; set => actions = value; }

    public TextAsset json_file;

    public ActionFile getActionFile()
    {
        ActionFile actionFile = new ActionFile();
        actionFile.actions = actions;
        return actionFile;
    }

    public void FromActionFile(ActionFile actionFile)
    {
        actions = actionFile.actions;
    }

    void Awake()
    {
        if (json_file != null)
        {
            LoadActionFile();
        }
    }

    public void LoadActionFile()
    {
        ActionFile actions = JsonUtility.FromJson<ActionFile>(json_file.text);
        FromActionFile(actions);
    }

    public void AddAction()
    {
        actions.Add(new DynamicAction("New Action"));
    }

    public void AddAction(DynamicAction action)
    {
        actions.Add(action);
    }

    public void DeleteAction(DynamicAction action)
    {
        actions.Remove(action);
    }
}

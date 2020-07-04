using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableNewSubactionCallback : UndoableCallback
{
    [SerializeField] private NewSubactionButton subactionSource;
    [SerializeField] private List<SubactionData> subactionDestination;

    [SerializeField] private Stack<SubactionData> undoList = new Stack<SubactionData>();
    [SerializeField] private Stack<SubactionData> redoList = new Stack<SubactionData>();

    public void Execute()
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        SubactionData subData = subactionSource.GetSubaction();
        undoList.Push(subData);

        DynamicAction actionToModify = LESelectedAction.instance.CurrentAction;
        string categoryName = PanelNavigator.PanelNavigators["ActionCategory"].getCurrentPanel().GetComponent<SubactionCategoryPanel>().categoryName;
        subactionDestination = actionToModify.subactionCategories[categoryName];

        subactionDestination.Add(subData);
        
        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        SubactionData data = undoList.Pop();
        redoList.Push(data);
        subactionDestination.Remove(data);
    }

    public override void Redo()
    {
        SubactionData data = redoList.Pop();
        undoList.Push(data);
        subactionDestination.Add(data);
    }
}

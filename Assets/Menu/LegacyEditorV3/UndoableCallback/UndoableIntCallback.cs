using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UndoableIntCallback : UndoableCallback
{
    public IntCallback getValue;
    public IntEvent setValue;

    [SerializeField] private Stack<int> undoList = new Stack<int>();
    [SerializeField] private Stack<int> redoList = new Stack<int>();

    public void Execute(int data)
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(getValue.Invoke()); //Push the current value on the redo stack
        int data = undoList.Pop(); //Get the top of the undo stack
        setValue?.Invoke(data); //Set the value
    }

    public override void Redo()
    {
        int data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value
    }
}

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

[System.Serializable]
public class IntCallback : SerializableCallback<int> { }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyEditorController : MonoBehaviour
{
    public static LegacyEditorController current;

    public FighterInfo info;

    public Stack<UndoableCallback> undoStack = new Stack<UndoableCallback>();
    public Stack<UndoableCallback> redoStack = new Stack<UndoableCallback>();

    void OnEnable()
    {
        current = this;
    }

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Undo();
            } else if (Input.GetKeyDown(KeyCode.Y))
            {
                Redo();
            }
        } else
        {

        }
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            UndoableCallback callback = undoStack.Pop();
            callback.Undo();
            redoStack.Push(callback);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            UndoableCallback callback = redoStack.Pop();
            callback.Redo();
            undoStack.Push(callback);
        }
    }

    public static void ExecuteCallback(UndoableCallback callback)
    {
        current.redoStack.Clear();
        current.undoStack.Push(callback);
    }
}

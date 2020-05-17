using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LegacyEditorController : MonoBehaviour
{
    public static LegacyEditorController current;

    public string workingDirectoryName;
    public DirectoryInfo workingDirectory;

    public Stack<UndoableCallback> undoStack = new Stack<UndoableCallback>();
    public Stack<UndoableCallback> redoStack = new Stack<UndoableCallback>();

    private MonoFighter fighter;
    private MonoSpriteInfo spriteInfo;

    private void Awake()
    {
        fighter = GetComponent<MonoFighter>();
        spriteInfo = GetComponent<MonoSpriteInfo>();
    }

    void OnEnable()
    {
        current = this;
        workingDirectory = FileLoader.GetFighterDir(workingDirectoryName);
        Debug.Log(workingDirectory.FullName);
    }

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Undo();
                }
                else if (Input.GetKeyDown(KeyCode.Y))
                {
                    Redo();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SaveAll();
                }
            }
        } else
        {

        }
    }

    public void SaveAll()
    {
        Debug.Log(JsonUtility.ToJson(fighter.getFighterInfo()));
        Debug.Log(JsonUtility.ToJson(spriteInfo.getSpriteInfo()));
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

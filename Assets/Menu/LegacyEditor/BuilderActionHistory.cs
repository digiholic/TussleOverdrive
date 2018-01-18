using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderActionHistory : MonoBehaviour {
    public static Stack<BuilderAction> undoList = new Stack<BuilderAction>();
    public static Stack<BuilderAction> redoList = new Stack<BuilderAction>();

	// Use this for initialization
	void Start () {
		
	}

    public static void Undo()
    {
        Debug.Log("Undoing");
        if (undoList.Count > 0)
        {
            BuilderAction a = undoList.Pop();
            a.Undo();
            redoList.Push(a);
        }
    }
    
    public static void Redo()
    {
        Debug.Log("Redoing");
        if (redoList.Count > 0)
        {
            BuilderAction a = redoList.Pop();
            a.Execute();
            undoList.Push(a);
        }
    }

    public static void ExecuteAction(BuilderAction a)
    {
        redoList.Clear();
        a.Execute();
        undoList.Push(a);
    }
}

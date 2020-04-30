using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UndoableCallback : MonoBehaviour {
    public abstract void Undo();
    public abstract void Redo();
}

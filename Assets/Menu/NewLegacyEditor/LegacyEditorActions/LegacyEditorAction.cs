using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LegacyEditorAction : ScriptableObject {

    public abstract void execute();
    public abstract void undo();
}

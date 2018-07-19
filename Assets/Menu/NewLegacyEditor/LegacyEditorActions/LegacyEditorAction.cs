using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LegacyEditorAction : ScriptableObject {

    //There is also a specific "init" function that takes different parameters and shouldn't be inherited
    //Looks like this:
    //public void init(...);
    public abstract void execute();
    public abstract void undo();
}

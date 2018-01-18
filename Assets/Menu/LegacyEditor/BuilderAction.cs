using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuilderAction{
    public abstract void Execute();
    public abstract void Undo();
}

public class ChangeWindowAction : BuilderAction
{
    public string new_window;
    public string old_window;

    public ChangeWindowAction(string _new, string _old)
    {
        new_window = _new;
        old_window = _old;
    }

    public override void Execute()
    {
        LegacyEditor.ChangeWindow(new_window);
    }

    public override void Undo()
    {
        LegacyEditor.ChangeWindow(old_window);
    }
}
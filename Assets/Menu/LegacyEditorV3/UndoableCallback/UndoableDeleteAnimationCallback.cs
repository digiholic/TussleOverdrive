using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableDeleteAnimationCallback : UndoableCallback
{
    [SerializeField] private AnimationPane animationSource;
    [SerializeField] private MonoSpriteInfo animationDestination;

    [SerializeField] private Stack<AnimationDefinition> undoList = new Stack<AnimationDefinition>();
    [SerializeField] private Stack<AnimationDefinition> redoList = new Stack<AnimationDefinition>();

    public void Execute()
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        undoList.Push(animationSource.animation); //Push the old value to the top of our undo stack
        animationDestination.DeleteAnimation(animationSource.animation);

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(animationSource.animation); //Push the current value on the redo stack
        AnimationDefinition data = undoList.Pop(); //Get the top of the undo stack
        animationDestination.AddAnimation(data);
    }

    public override void Redo()
    {
        AnimationDefinition data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(animationSource.animation); //Push the old value to the top of our undo stack
        animationDestination.DeleteAnimation(data);
    }
}

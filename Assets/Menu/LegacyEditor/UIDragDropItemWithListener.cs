using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDragDropItemWithListener : MonoBehaviour
{
    public UnityEvent onDragDropStartEvent;
    public UnityEvent onDragDropMoveEvent;
    public UnityEvent onDragDropReleaseEvent;
    public UnityEvent onDragDropEndEvent;

    private void Awake()
    {
        //Initialize the events if they're null to avoid crashing
        if (onDragDropStartEvent == null) onDragDropStartEvent = new UnityEvent();
        if (onDragDropMoveEvent == null) onDragDropMoveEvent = new UnityEvent();
        if (onDragDropReleaseEvent == null) onDragDropReleaseEvent = new UnityEvent();
        if (onDragDropEndEvent == null) onDragDropEndEvent = new UnityEvent();
    }

    //Call the methods set in the inspector
    protected void OnDragDropStartEvent() { onDragDropStartEvent.Invoke(); }
    protected void OnDragDropMoveEvent() { onDragDropMoveEvent.Invoke(); }
    protected void OnDragDropReleaseEvent() { onDragDropReleaseEvent.Invoke(); }
    protected void OnDragDropEndEvent() { onDragDropEndEvent.Invoke(); }
}

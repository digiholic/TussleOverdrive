using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ViewerClickable : MonoBehaviour
{
    [Tooltip("Which mouse button to listen for, 0 is left, 1 is right, 2 is middle (usually). You can have multiple clickables if it should respond to multiple buttons")]
    public int mouseButtonToListenFor = 0;

    public UnityEvent onHoverStart;
    public UnityEvent whileHovered;
    public UnityEvent onHoverEnd;
    public UnityEvent onClicked;
    public UnityEvent onReleased;
    

    [SerializeField] Camera viewerCamera;
    private Collider col;

    private bool hovered;
    void Awake()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        bool oldHoverState = hovered;
        hovered = false;

        RaycastHit hit;
        Ray ray = viewerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                hovered = true;
                //Call the events based on the previous state and whether the button has been clicked or released
                if (!oldHoverState) onHoverStart?.Invoke();
                whileHovered?.Invoke();
                if (Input.GetMouseButtonDown(mouseButtonToListenFor)) onClicked?.Invoke();
                if (Input.GetMouseButtonUp(mouseButtonToListenFor)) {
                    Debug.Log("Releasing");
                    onReleased?.Invoke();
                }
            }
        }

        //If we were hovering this before but aren't any more, call the onhoverend event
        if (oldHoverState && !hovered){
            onHoverEnd?.Invoke();
        }
    }
}

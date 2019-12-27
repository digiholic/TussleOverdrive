// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649

namespace Rewired.Demos {

    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    [AddComponentMenu("")]
    [RequireComponent(typeof(Image))]
    public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

        public bool allowMouseControl = true;
        public int radius = 50;
        
        private Vector2 origAnchoredPosition;
        private Vector3 origWorldPosition;
        private Vector2 origScreenResolution;
        private ScreenOrientation origScreenOrientation;

        [NonSerialized] // do not serialize this in case of an editor recompile
        private bool hasFinger;
        [NonSerialized] // do not serialize this in case of an editor recompile
        private int lastFingerId;

        public Vector2 position {
            get;
            private set;
        }

        private void Start() {
            if(SystemInfo.deviceType == DeviceType.Handheld) allowMouseControl = false; // disable mouse control on touch devices
            StoreOrigValues();
        }

        private void Update() {
            // Watch for changes that require recalculating the starting position
            if(Screen.width != origScreenResolution.x ||
                Screen.height != origScreenResolution.y ||
                Screen.orientation != origScreenOrientation) {
                    Restart();
                    StoreOrigValues();
            }
        }

        private void Restart() {
            hasFinger = false;
            (transform as RectTransform).anchoredPosition = origAnchoredPosition;
            position = Vector2.zero;
        }

        private void StoreOrigValues() {
            origAnchoredPosition = (transform as RectTransform).anchoredPosition;
            origWorldPosition = transform.position;
            origScreenResolution = new Vector2(Screen.width, Screen.height);
            origScreenOrientation = Screen.orientation;
        }

        private void UpdateValue(Vector3 value) {
            var delta = origWorldPosition - value;
            delta.y = -delta.y;
            delta /= radius;
            position = new Vector2(-delta.x, delta.y);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            if(hasFinger) return; // already a finger controlling this joystick
            if(!allowMouseControl && IsMousePointerId(eventData.pointerId)) return;
            
            hasFinger = true;
            lastFingerId = eventData.pointerId;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            if(eventData.pointerId != lastFingerId) return; // ignore if it doesn't match the finger id
            if(!allowMouseControl && IsMousePointerId(eventData.pointerId)) return;

            Restart();
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            if(!hasFinger || eventData.pointerId != lastFingerId) return; // not the right finger

            // Find the change in position from the center point of the joystick to the current finger position
            Vector3 delta = new Vector3(
                eventData.position.x - origWorldPosition.x,
                eventData.position.y - origWorldPosition.y
            );

            // Clamp the delta to the joystick's max radius
            delta = Vector3.ClampMagnitude(delta, radius);

            // Find the new joystick position
            Vector3 newPos = origWorldPosition + delta;

            transform.position = newPos; // set the position
            UpdateValue(newPos); // update the output value
        }

        private static bool IsMousePointerId(int id) {
            return id == PointerInputModule.kMouseLeftId ||
                id == PointerInputModule.kMouseRightId ||
                id == PointerInputModule.kMouseMiddleId;
        }
    }
}
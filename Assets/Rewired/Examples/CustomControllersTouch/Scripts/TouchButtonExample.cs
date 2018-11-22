// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649

namespace Rewired.Demos {

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    [AddComponentMenu("")]
    [RequireComponent(typeof(Image))]
    public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        public bool allowMouseControl = true;

        public bool isPressed {
            get;
            private set;
        }

        private void Awake() {
            if(SystemInfo.deviceType == DeviceType.Handheld) allowMouseControl = false; // disable mouse control on touch devices
        }

        private void Restart() {
            isPressed = false;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            if(!allowMouseControl && IsMousePointerId(eventData.pointerId)) return;
            isPressed = true;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            if(!allowMouseControl && IsMousePointerId(eventData.pointerId)) return;
            isPressed = false;
        }

        private static bool IsMousePointerId(int id) {
            return id == PointerInputModule.kMouseLeftId ||
                id == PointerInputModule.kMouseRightId ||
                id == PointerInputModule.kMouseMiddleId;
        }
    }
}
// Modified by Augie Maddox from Joystick
// Source: http://wiki.unity3d.com/index.php?title=button

namespace Rewired.Demos {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    [RequireComponent(typeof(GUITexture))]
    public class TouchButtonExample : MonoBehaviour {

        public bool allowMouseControl = true;

        // Current tap count 
        public int tapCount = -1;

        // Finger last used on this button 
        private int lastFingerId = -1;
        // How much time there is left for a tap to occur 
        private float tapTimeWindow;
        private float firstDeltaTime;

        // TouchButton graphic 
        private GUITexture gui;
        // Default position / extents of the button graphic 
        private Rect defaultRect;
        // Boundary for button graphic 
        private Boundary guiBoundary = new Boundary();
        // Offset to apply to touch input 
        private Vector2 guiTouchOffset;
        // Center of button 
        private Vector2 guiCenter;

        public bool isFingerDown {
            get {
                return (lastFingerId != -1);
            }
        }

        public bool isPressed {
            get;
            private set;
        }

        private bool mouseActive;

        private int lastScreenWidth;
        private Rect origPixelInset;
        private Vector3 origTransformPosition;

        // A static collection of all buttons 
        private static List<TouchButtonExample> buttons;
        // Time allowed between taps 
        private static float tapTimeDelta = 0.3f;
        
        private void Reset() {
        }

        private void Awake() {
            if(SystemInfo.deviceType == DeviceType.Handheld) allowMouseControl = false; // disable mouse control on touch devices

            gui = GetComponent<GUITexture>();
            if(gui.texture == null) {
                Debug.LogError("TouchButton object requires a valid texture!");
                gameObject.SetActive(false);
                return;
            }

            origPixelInset = gui.pixelInset; // store the original pixel inset
            origTransformPosition = transform.position; // store the original transform position

            RefreshPosition();

        }

        private void RefreshPosition() {
            // Store the default rect for the gui, so we can snap back to it 
            defaultRect = origPixelInset;
            
            defaultRect.x += origTransformPosition.x * Screen.width;
            defaultRect.y += origTransformPosition.y * Screen.height;

            gui.pixelInset = defaultRect; // make sure GUI is in default position

            transform.position = new Vector3(0, 0, transform.position.z);

            // This is an offset for touch input to match with the top left corner of the GUI 
            guiTouchOffset.x = defaultRect.width * 0.5f;
            guiTouchOffset.y = defaultRect.height * 0.5f;

            // Cache the center of the GUI, since it doesn't change 
            guiCenter.x = defaultRect.x + guiTouchOffset.x;
            guiCenter.y = defaultRect.y + guiTouchOffset.y;

            // Let's build the GUI boundary, so we can clamp button movement 
            guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
            guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
            guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
            guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;

            lastScreenWidth = Screen.width; // store the original screen width in case it changes
            Restart();
        }

        public void Enable() {
            enabled = true;
        }

        public void Disable() {
            enabled = false;
        }

        public void Restart() {
            // Release the finger control and set the button back to the default position 
            gui.pixelInset = defaultRect;
            lastFingerId = -1;
            isPressed = false;

            mouseActive = false;
        }

        private void Update() {
            if(lastScreenWidth != Screen.width) RefreshPosition(); // update the GUI positions if screen orientation changes
            if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.

            int count;

            if(mouseActive && !ReInput.controllers.Mouse.GetButton(0)) mouseActive = false; // check for mouse release

            // Choose between mouse or touch control -- mouse overrides touch.
            if(allowMouseControl && (mouseActive || (ReInput.controllers.Mouse.GetButtonDown(0)) && gui.HitTest(ReInput.controllers.Mouse.screenPosition))) {
                count = 1;
                mouseActive = true;
            } else {
                count = ReInput.touch.touchCount;
                if(mouseActive) mouseActive = false;
            }

            // Adjust the tap time window while it still available 
            if(tapTimeWindow > 0) {
                tapTimeWindow -= Time.deltaTime;
            } else {
                tapCount = 0;
            }

            if(count == 0) {
                Restart();
            } else {
                for(int i = 0; i < count; i++) {
                    
                    Vector2 tPosition;
                    int tFingerId;
                    int tTapCount;
                    TouchPhase tPhase;

                    if(mouseActive) {
                        tPosition = ReInput.controllers.Mouse.screenPosition;
                        tFingerId = 0;
                        tTapCount = 1;
                        tPhase = TouchPhase.Moved;
                    } else {
                        Touch touch = ReInput.touch.GetTouch(i);
                        tPosition = touch.position;
                        tFingerId = touch.fingerId;
                        tTapCount = touch.tapCount;
                        tPhase = touch.phase;
                    }

                    if(!gui.HitTest(tPosition)) { // touch is outside area
                        continue;
                    }

                    // Latch the finger if this is a new touch 
                    if(lastFingerId == -1 || lastFingerId != tFingerId) {
                        
                        lastFingerId = tFingerId;

                        // Accumulate taps if it is within the time window 
                        if(tapTimeWindow > 0) {
                            tapCount++;
                        } else {
                            tapCount = 1;
                            tapTimeWindow = tapTimeDelta;
                        }
                    }

                    if(lastFingerId == tFingerId) {
                        /*
                            Override the tap count with what the iOS SDK reports if it is greater.
                            This is a workaround, since the iOS SDK does not currently track taps
                            for multiple touches.
                        */
                        if(tTapCount > tapCount) {
                            tapCount = tTapCount;
                        }

                        isPressed = true;

                        if(tPhase == TouchPhase.Ended || tPhase == TouchPhase.Canceled) {
                            Restart();
                        }
                    }
                }
            }
        }

        private class Boundary {
            public Vector2 min = Vector2.zero;
            public Vector2 max = Vector2.zero;
        }
    }
}
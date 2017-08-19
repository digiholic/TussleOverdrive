// Author: Unity Technologies.
// Translated into C# by Juan Manuel Palacios, with a slightly modified API. 
// Modified by Augie Maddox to include mouse support.
// Source: http://wiki.unity3d.com/index.php?title=Joystick

namespace Rewired.Demos {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    [RequireComponent(typeof(GUITexture))]
    public class TouchJoystickExample : MonoBehaviour {

        public bool allowMouseControl = true;

        // Is this joystick a TouchPad? 
        public bool touchPad = false;
        // In case the joystick is a touchPad, should its GUI be faded when inactive? 
        public bool fadeGUI = false;
        // Control when position is output 
        public Vector2 deadZone = Vector2.zero;
        // Normalize output after the dead-zone? 
        public bool normalize = false;
        // Current tap count 
        public int tapCount = -1;

        // The touchZone of the joystick 
        private Rect touchZone;

        // Finger last used on this joystick 
        private int lastFingerId = -1;
        // How much time there is left for a tap to occur 
        private float tapTimeWindow;
        private Vector2 fingerDownPos;
        /*
         * Currently unused.
        private float fingerDownTime;
        */
        private float firstDeltaTime;

        // TouchJoystick graphic 
        private GUITexture gui;
        // Default position / extents of the joystick graphic 
        private Rect defaultRect;
        // Boundary for joystick graphic 
        private Boundary guiBoundary = new Boundary();
        // Offset to apply to touch input 
        private Vector2 guiTouchOffset;
        // Center of joystick 
        private Vector2 guiCenter;

        [NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
        private bool initialized;

        public bool isFingerDown {
            get {
                return (lastFingerId != -1);
            }
        }

        public int latchedFinger {
            set {
                // If another joystick has latched this finger, then we must release it 
                if(lastFingerId == value) {
                    Restart();
                }
            }
        }

        // The position of the joystick on the screen ([-1, 1] in x,y) for clients to read. 
        public Vector2 position {
            get;
            private set;
        }

        private bool mouseActive;

        private int lastScreenWidth;
        private Rect origPixelInset;
        private Vector3 origTransformPosition;

        // A static collection of all joysticks 
        private static List<TouchJoystickExample> joysticks;
        // Has the joysticks collection been enumerated yet? 
        private static bool enumeratedTouchJoysticks = false;
        // Time allowed between taps 
        private static float tapTimeDelta = 0.3f;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            ReInput.EditorRecompileEvent += OnEditorRecompile; // subscribe to recompile event so we can handle recompiling in the editor

            if(SystemInfo.deviceType == DeviceType.Handheld) allowMouseControl = false; // disable mouse control on touch devices

            gui = GetComponent<GUITexture>();
            if(gui.texture == null) {
                Debug.LogError("TouchJoystick object requires a valid texture!");
                gameObject.SetActive(false);
                return;
            }

            if(!enumeratedTouchJoysticks) {
                try {
                    // Collect all joysticks in the game, so we can relay finger latching messages 
                    TouchJoystickExample[] objs = (TouchJoystickExample[])GameObject.FindObjectsOfType(typeof(TouchJoystickExample));
                    joysticks = new List<TouchJoystickExample>(objs.Length);
                    foreach(TouchJoystickExample obj in objs) {
                        joysticks.Add(obj);
                    }
                    enumeratedTouchJoysticks = true;
                } catch(Exception exp) {
                    Debug.LogError("Error collecting TouchJoystick objects: " + exp.Message);
                    throw;
                }
            }

            origPixelInset = gui.pixelInset; // store the original pixel inset
            origTransformPosition = transform.position; // store the original transform position

            RefreshPosition();

            initialized = true;
        }

        private void RefreshPosition() {         
            // Store the default rect for the gui, so we can snap back to it 
            defaultRect = origPixelInset;
            
            defaultRect.x += origTransformPosition.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5f;
            defaultRect.y += origTransformPosition.y * Screen.height;// - Screen.height * 0.5f;

            gui.pixelInset = defaultRect; // make sure GUI is in default position

            transform.position = new Vector3(0, 0, transform.position.z);

            if(touchPad) {
                // Use the rect from the gui as our touchZone 
                touchZone = defaultRect;
            } else {
                // This is an offset for touch input to match with the top left corner of the GUI 
                guiTouchOffset.x = defaultRect.width * 0.5f;
                guiTouchOffset.y = defaultRect.height * 0.5f;

                // Cache the center of the GUI, since it doesn't change 
                guiCenter.x = defaultRect.x + guiTouchOffset.x;
                guiCenter.y = defaultRect.y + guiTouchOffset.y;

                // Let's build the GUI boundary, so we can clamp joystick movement 
                guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
                guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
                guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
                guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
            }

            lastScreenWidth = Screen.width; // store the original screen width in case it changes
            Restart();
        }

        private void OnEditorRecompile() {
            initialized = false; // prevent this script from processing while recompiling
            enumeratedTouchJoysticks = false;
        }

        public void Enable() {
            enabled = true;
        }

        public void Disable() {
            enabled = false;
        }

        public void Restart() {
            // Release the finger control and set the joystick back to the default position 
            gui.pixelInset = defaultRect;
            lastFingerId = -1;
            position = Vector2.zero;
            fingerDownPos = Vector2.zero;

            if(touchPad && fadeGUI) {
                gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 0.025f);
            }

            mouseActive = false;
        }

        private void Update() {
            if(lastScreenWidth != Screen.width) RefreshPosition(); // update the GUI positions if screen orientation changes
            if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
            if(!initialized) Initialize(); // Reinitialize after a recompile in the editor

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

                    Vector2 guiTouchPos = tPosition - guiTouchOffset;
                    
                    bool shouldLatchFinger = false;
                    if(touchPad && touchZone.Contains(tPosition)) {
                        shouldLatchFinger = true;
                    } else if(gui.HitTest(tPosition)) {
                        shouldLatchFinger = true;
                    }

                    // Latch the finger if this is a new touch 
                    if(shouldLatchFinger && (lastFingerId == -1 || lastFingerId != tFingerId)) {
                        
                        if(touchPad) {
                            if(fadeGUI) {
                                gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 0.15f);
                            }
                            lastFingerId = tFingerId;
                            fingerDownPos = tPosition;
                            /*
                             * Currently unused.
                            fingerDownTime = Time.time;
                            */
                        }

                        lastFingerId = tFingerId;

                        // Accumulate taps if it is within the time window 
                        if(tapTimeWindow > 0) {
                            tapCount++;
                        } else {
                            tapCount = 1;
                            tapTimeWindow = tapTimeDelta;
                        }

                        // Tell other joysticks we've latched this finger 
                        foreach(TouchJoystickExample j in joysticks) {
                            if(j == this) {
                                continue;
                            }
                            j.latchedFinger = tFingerId;
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

                        if(touchPad) {
                            // For a touchpad, let's just set the position directly based on distance from initial touchdown 
                            position = new Vector2
                                (
                                  Mathf.Clamp((tPosition.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1),
                                  Mathf.Clamp((tPosition.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1)
                                );
                        } else {
                            // Change the location of the joystick graphic to match where the touch is 

                            gui.pixelInset = new Rect
                                (
                                  Mathf.Clamp(guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x),
                                  Mathf.Clamp(guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y),
                                  gui.pixelInset.width,
                                  gui.pixelInset.height
                                );
                        }

                        if(tPhase == TouchPhase.Ended || tPhase == TouchPhase.Canceled) {
                            Restart();
                        }
                    }
                }
            }

            if(!touchPad) {
                // Get a value between -1 and 1 based on the joystick graphic location 
                position = new Vector2
                    (
                      (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x,
                      (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y
                    );
            }

            // Adjust for dead zone 
            float absoluteX = Mathf.Abs(position.x);
            float absoluteY = Mathf.Abs(position.y);

            if(absoluteX < deadZone.x) {
                // Report the joystick as being at the center if it is within the dead zone 
                position = new Vector2(0, position.y);
            } else if(normalize) {
                // Rescale the output after taking the dead zone into account 
                position = new Vector2(Mathf.Sign(position.x) * (absoluteX - deadZone.x) / (1 - deadZone.x), position.y);
            }

            if(absoluteY < deadZone.y) {
                // Report the joystick as being at the center if it is within the dead zone 
                position = new Vector2(position.x, 0);
            } else if(normalize) {
                // Rescale the output after taking the dead zone into account 
                position = new Vector2(position.x, Mathf.Sign(position.y) * (absoluteY - deadZone.y) / (1 - deadZone.y));
            }
        }

        private class Boundary {
            public Vector2 min = Vector2.zero;
            public Vector2 max = Vector2.zero;
        }
    }
}
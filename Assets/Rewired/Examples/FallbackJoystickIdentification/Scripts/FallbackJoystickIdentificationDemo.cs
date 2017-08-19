// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

// This example shows how to have the user manually identify joysticks by name to assit with a Unity
// bug where joysticks cannot be associated with a Unity joystick ID without manual intervention when
// using Unity as the input source (as opposed to native input).

// NOTE: This only affects the Windows Standalone and Windows Webplayer platforms and was patched by Unity in 4.6.3p1,
// so this is no longer required in Unity 4.x versions after 4.6.3p1.
// Currently, Unity 5.x does not implement this fix yet, so this test is recommended.

namespace Rewired.Demos {

    using UnityEngine;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    public class FallbackJoystickIdentificationDemo : MonoBehaviour {

        // Consts
        private const float windowWidth = 250.0f;
        private const float windowHeight = 250.0f;
        private const float inputDelay = 1.0f;

        // Working  vars
        private bool identifyRequired;
        private Queue<Joystick> joysticksToIdentify;
        private float nextInputAllowedTime;
        private GUIStyle style;

        private void Awake() {
            if(!ReInput.unityJoystickIdentificationRequired) return; // this platform does not require manual joystick identificaion

            // Subscribe to device change events
            ReInput.ControllerConnectedEvent += JoystickConnected;
            ReInput.ControllerDisconnectedEvent += JoystickDisconnected; // this event is called after joystick is fully disconnected and removed from lists

            IdentifyAllJoysticks();
        }

        private void JoystickConnected(ControllerStatusChangedEventArgs args) {
            // Identify all joysticks on connect or disconnect because ids are not reliable in Unity
            IdentifyAllJoysticks();
        }

        private void JoystickDisconnected(ControllerStatusChangedEventArgs args) {
            // Identify all joysticks on connect or disconnect because ids are not reliable in Unity
            IdentifyAllJoysticks();
        }

        public void IdentifyAllJoysticks() {
            // Reset each time in case user changes joysticks while dialog is open
            Reset();

            // Check if there are any joysticks
            if(ReInput.controllers.joystickCount == 0) return; // no joysticks, nothing to do

            // Get current Joysticks
            Joystick[] joysticks = ReInput.controllers.GetJoysticks();
            if(joysticks == null) return;

            // Set flag to enable identification mode
            identifyRequired = true;

            // Create a queue out of the joysticks array
            joysticksToIdentify = new Queue<Joystick>(joysticks);

            // Set the time for accepting input again
            SetInputDelay();
        }

        private void SetInputDelay() {
            // Prevent user input for a period of time after each identification to handle button hold problem
            nextInputAllowedTime = Time.time + inputDelay;
        }

        private void OnGUI() {
            if(!identifyRequired) return;
            if(joysticksToIdentify == null || joysticksToIdentify.Count == 0) {
                Reset();
                return;
            }

            // Draw dialog window
            Rect centerWindowRect = new Rect(Screen.width * 0.5f - windowWidth * 0.5f, Screen.height * 0.5f - windowHeight * 0.5f, windowWidth, windowHeight); // create a cetered window rect
            GUILayout.Window(0, centerWindowRect, DrawDialogWindow, "Joystick Identification Required"); // draw the window
            GUI.FocusWindow(0); // focus the window

            // Do not allow input during input delay to filter out holding a button down and assigning all joysticks to a single joystick id
            if(Time.time < nextInputAllowedTime) return;

            // Poll for a joystick button press to identify the joystick
            if(!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(joysticksToIdentify.Peek().id, 0.8f, false)) {
                return; // no input detected
            }

            // Remove the joystick from the queue now that we've used it
            joysticksToIdentify.Dequeue();

            // Renew the input delay time after press
            SetInputDelay();

            // Finish up if the queue is empty
            if(joysticksToIdentify.Count == 0) {
                Reset(); // done
            }
        }

        private void DrawDialogWindow(int windowId) {
            if(!identifyRequired) return; // window displays 1 frame after it is closed, so this is required to prevent null references below

            // Set up a temporary style with word wrap
            if(style == null) {
                style = new GUIStyle(GUI.skin.label);
                style.wordWrap = true;
            }

            // Draw the window contents
            GUILayout.Space(15);
            GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", style);
            Joystick joystick = joysticksToIdentify.Peek();
            GUILayout.Label("Press any button on \"" + joystick.name + "\" now.", style);

            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Skip")) {
                joysticksToIdentify.Dequeue();
                return;
            }
        }

        private void Reset() {
            joysticksToIdentify = null;
            identifyRequired = false;
        }
    }
}
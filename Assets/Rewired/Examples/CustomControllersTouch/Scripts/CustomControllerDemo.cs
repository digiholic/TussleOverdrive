// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.Demos {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Rewired;

    /* IMPORTANT NOTE: Touch control is now available through using the Touch Controller components. Please see
     * the documentation on Touch Controls for more information: http://guavaman.com/rewired/docs/TouchControls.html
     * 
     * Demonstrates using a CustomController to drive input. A custom controller allows you to set your own sources for axis
     * and button input. This could be any type of controller or virtual controller. Anything that can return a float or a
     * bool value can be used as an element source.
     * 
     * This example creates two on-screen thumb pads which will control the two characters. You can use the mouse to control
     * the thumb pads if you do not have a touch screen.
    */

    [AddComponentMenu("")]
    [RequireComponent(typeof(Demos.TouchControllerExample))]
    public class CustomControllerDemo : MonoBehaviour {

        public int playerId;
        public string controllerTag;
        public bool useUpdateCallbacks;

        private int buttonCount;
        private int axisCount;
        private float[] axisValues;
        private bool[] buttonValues;

        private Demos.TouchControllerExample touchController;
        private CustomController controller;

        [NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
        private bool initialized;

        private void Awake() {
            if(SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != ScreenOrientation.Landscape) { // set screen to landscape mode
                Screen.orientation = ScreenOrientation.Landscape;
            }
            Initialize();
        }

        private void Initialize() {
            // Subscribe to the input source update event so we can update our source element data before controllers are updated
            ReInput.InputSourceUpdateEvent += OnInputSourceUpdate;

            // Get the touch controller
            touchController = GetComponent<Demos.TouchControllerExample>();

            // Get expected element counts
            axisCount = touchController.joysticks.Length * 2; // 2 axes per stick
            buttonCount = touchController.buttons.Length;

            // Set up arrays to store our current source element values
            axisValues = new float[axisCount];
            buttonValues = new bool[buttonCount];

            // Find the controller we want to manage
            Player player = ReInput.players.GetPlayer(playerId); // get the player
            controller = player.controllers.GetControllerWithTag<CustomController>(controllerTag); // get the controller

            if(controller == null) {
                Debug.LogError("A matching controller was not found for tag \"" + controllerTag + "\"");
            }

            // Verify controller has the number of elements we're expecting
            if(controller.buttonCount != buttonValues.Length || controller.axisCount != axisValues.Length) { // controller has wrong number of elements
                Debug.LogError("Controller has wrong number of elements!");
            }

            // Callback Update Method:
            // Set callbacks to retrieve current element values.
            // This is a different way of updating the element values in the controller.
            // You set an update function for axes and buttons and these functions will be called
            // to retrieve the current source element values on every update loop in which input is updated.
            if(useUpdateCallbacks && controller != null) {
                controller.SetAxisUpdateCallback(GetAxisValueCallback);
                controller.SetButtonUpdateCallback(GetButtonValueCallback);
            }

            initialized = true;
        }

        private void Update() {
            if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
            if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
        }

        private void OnInputSourceUpdate() {
            // This will be called every time the input sources are updated
            // It may be called in Update, Fixed Update, and/or OnGUI depending on the UpdateLoop setting in InputManager
            // If you need to know what update loop this was called in, check currentUpdateLoop

            // Update the source element values from our source, whatever that may be
            GetSourceAxisValues();
            GetSourceButtonValues();

            // Set the current values directly in the controller
            if(!useUpdateCallbacks) { // if not using update callbacks, set the values directly, otherwise controller values will be updated via callbacks
                SetControllerAxisValues();
                SetControllerButtonValues();
            }
        }

        // Get the current values from the source elements. 

        private void GetSourceAxisValues() {
            // Get the current element values from our source and store them
            for(int i = 0; i < axisValues.Length; i++) {
                if(i % 2 != 0) {// odd
                    axisValues[i] = touchController.joysticks[i/2].position.y;
                } else { // even
                    axisValues[i] = touchController.joysticks[i / 2].position.x;
                }
            }
        }

        private void GetSourceButtonValues() {
            // Get the current element values from our source and store them
            for(int i = 0; i < buttonValues.Length; i++) {
                buttonValues[i] = touchController.buttons[i].isPressed;
            }
        }

        // One way to set values in the controller is to set them directly with SetAxisValue and SetButtonValue
        // If you use this method, you should make sure you call it from the InputSourceUpdateEvent because this
        // event will be called once for every update loop in which input is updated.

        private void SetControllerAxisValues() {
            // Set the element values directly in the controller
            for(int i = 0; i < axisValues.Length; i++) {
                controller.SetAxisValue(i, axisValues[i]);
            }
        }

        private void SetControllerButtonValues() {
            // Set the element values directly in the controller
            for(int i = 0; i < buttonValues.Length; i++) {
                controller.SetButtonValue(i, buttonValues[i]);
            }
        }

        // Callbacks

        private float GetAxisValueCallback(int index) {
            // This will be called by each axis element in the Custom Controller when updating its raw value
            // Get the current value from the source axis at index
            if(index >= axisValues.Length) return 0.0f;
            return axisValues[index];
        }

        private bool GetButtonValueCallback(int index) {
            // This will be called by each button element in the Custom Controller when updating its raw value
            // Get the current value from the source button at index
            if(index >= buttonValues.Length) return false;
            return buttonValues[index];
        }

    }
}

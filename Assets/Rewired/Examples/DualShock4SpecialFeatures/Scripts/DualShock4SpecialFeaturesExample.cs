// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#region Defines
#if UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024 || UNITY_2025
#define UNITY_2020_PLUS
#endif
#if UNITY_2019 || UNITY_2020_PLUS
#define UNITY_2019_PLUS
#endif
#if UNITY_2018 || UNITY_2019_PLUS
#define UNITY_2018_PLUS
#endif
#if UNITY_2017 || UNITY_2018_PLUS
#define UNITY_2017_PLUS
#endif
#if UNITY_5 || UNITY_2017_PLUS
#define UNITY_5_PLUS
#endif
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_1_PLUS
#endif
#if UNITY_5_2 || UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_2_PLUS
#endif
#if UNITY_5_3_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_3_PLUS
#endif
#if UNITY_5_4_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_4_PLUS
#endif
#if UNITY_5_5_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_5_PLUS
#endif
#if UNITY_5_6_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_6_PLUS
#endif
#if UNITY_5_7_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_7_PLUS
#endif
#if UNITY_5_8_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_8_PLUS
#endif
#if UNITY_5_9_OR_NEWER || UNITY_2017_PLUS
#define UNITY_5_9_PLUS
#endif
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649
#pragma warning disable 0067
#endregion

namespace Rewired.Demos {
    using UnityEngine;
    using System.Collections.Generic;
	using Rewired.ControllerExtensions;

    [AddComponentMenu("")]
    public class DualShock4SpecialFeaturesExample : MonoBehaviour {

        private const int maxTouches = 2;

        public int playerId = 0;
        public Transform touchpadTransform;
        public GameObject lightObject;
        public Transform accelerometerTransform;

        private List<Touch> touches;
        private Queue<Touch> unusedTouches;
        private bool isFlashing;
        private GUIStyle textStyle;

        private Player player { get { return ReInput.players.GetPlayer(playerId); } }

        private void Awake() {
            InitializeTouchObjects();
        }

		private void Update() {
			if (!ReInput.isReady) return;
            
            // Get the first DS4 found assigned to the Player
            var ds4 = GetFirstDS4(player);
            if(ds4 != null) {

                // Set the model's rotation to match the controller's
                transform.rotation = ds4.GetOrientation();

                // Show touchpad touches
                HandleTouchpad(ds4);

                // Show accelerometer value
                Vector3 accelerometerValue = ds4.GetAccelerometerValue();
                accelerometerTransform.LookAt(accelerometerTransform.position + accelerometerValue);
            }

            if(player.GetButtonDown("CycleLight")) {
                SetRandomLightColor();
            }

            if(player.GetButtonDown("ResetOrientation")) {
                ResetOrientation();
            }

            if(player.GetButtonDown("ToggleLightFlash")) {
                if(isFlashing) {
                    StopLightFlash();
                } else {
                    StartLightFlash();
                }
                isFlashing = !isFlashing;
            }

            if (player.GetButtonDown("VibrateLeft")) {
                ds4.SetVibration(0, 1f, 1f);
            }

            if (player.GetButtonDown("VibrateRight")) {
                ds4.SetVibration(1, 1f, 1f);
            }
        }

        private void OnGUI() {
            if(textStyle == null) {
                textStyle = new GUIStyle(GUI.skin.label);
                textStyle.fontSize = 20;
                textStyle.wordWrap = true;
            }

            if(GetFirstDS4(player) == null) return; // no DS4 is assigned

            GUILayout.BeginArea(new Rect(200f, 100f, Screen.width - 400f, Screen.height - 200f));

            GUILayout.Label("Rotate the Dual Shock 4 to see the model rotate in sync.", textStyle);

            GUILayout.Label("Touch the touchpad to see them appear on the model.", textStyle);

            ActionElementMap aem;
            
            aem = player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ResetOrientation", true);
            if(aem != null) {
                GUILayout.Label("Press " + aem.elementIdentifierName + " to reset the orientation. Hold the gamepad facing the screen with sticks pointing up and press the button.", textStyle);
            }

            aem = player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "CycleLight", true);
            if(aem != null) {
                GUILayout.Label("Press " + aem.elementIdentifierName + " to change the light color.", textStyle);
            }

#if !UNITY_PS4

            // Light flash is not supported on the PS4 platform.
            aem = player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ToggleLightFlash", true);
            if(aem != null) {
                GUILayout.Label("Press " + aem.elementIdentifierName + " to start or stop the light flashing.", textStyle);
            }

#endif

            aem = player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateLeft", true);
            if (aem != null) {
                GUILayout.Label("Press " + aem.elementIdentifierName + " vibrate the left motor.", textStyle);
            }

            aem = player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateRight", true);
            if (aem != null) {
                GUILayout.Label("Press " + aem.elementIdentifierName + " vibrate the right motor.", textStyle);
            }

            GUILayout.EndArea();
		}

        private void ResetOrientation() {
            var ds4 = GetFirstDS4(player);
            if(ds4 != null) {
                ds4.ResetOrientation();
            }
        }

        private void SetRandomLightColor() {
            var ds4 = GetFirstDS4(player);
            if(ds4 != null) {
                Color color = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    1f
                );
                ds4.SetLightColor(color);
                lightObject.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        private void StartLightFlash() {
            // This is not supported on PS4 so get the Standalone DualShock4Extension
            DualShock4Extension ds4 = GetFirstDS4(player) as DualShock4Extension;
            if(ds4 != null) {
                ds4.SetLightFlash(0.5f, 0.5f);
                // Light flash is handled by the controller hardware itself and not software.
                // The current value cannot be obtained from the controller so it
                // cannot be reflected in the 3D model without just recreating the flash to approximate it.
            }
        }

        private void StopLightFlash() {
            // This is not supported on PS4 so get the Standalone DualShock4Extension
            DualShock4Extension ds4 = GetFirstDS4(player) as DualShock4Extension;
            if(ds4 != null) {
                ds4.StopLightFlash();
            }
        }

        private IDualShock4Extension GetFirstDS4(Player player) {
            foreach(Joystick j in player.controllers.Joysticks) {
                // Use the interface because it works for both PS4 and desktop platforms
                IDualShock4Extension ds4 = j.GetExtension<IDualShock4Extension>();
                if(ds4 == null) continue;
                return ds4;
            }
            return null;
        }

        private void InitializeTouchObjects() {

            touches = new List<Touch>(maxTouches);
            unusedTouches = new Queue<Touch>(maxTouches);

            // Setup touch objects
            for(int i = 0; i < maxTouches; i++) {
                Touch touch = new Touch();
                // Create spheres to reprensent touches
                touch.go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                touch.go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
#if UNITY_5_PLUS
                touch.go.transform.SetParent(touchpadTransform, true);
#else
                touch.go.transform.parent = touchpadTransform;
#endif
                touch.go.GetComponent<MeshRenderer>().material.color = i == 0 ? Color.red : Color.green;
                touch.go.SetActive(false);
                unusedTouches.Enqueue(touch);
            }
        }

        private void HandleTouchpad(IDualShock4Extension ds4) {
            // Expire old touches first
            for(int i = touches.Count - 1; i >= 0; i--) {
                Touch touch = touches[i];
                if(!ds4.IsTouchingByTouchId(touch.touchId)) { // the touch id is no longer valid
                    touch.go.SetActive(false); // disable the game object
                    unusedTouches.Enqueue(touch); // return to the pool
                    touches.RemoveAt(i); // remove from active touches list
                }
            }

            // Process new touches
            for(int i = 0; i < ds4.maxTouches; i++) {
                if(!ds4.IsTouching(i)) continue;
                int touchId = ds4.GetTouchId(i);
                Touch touch = touches.Find(x => x.touchId == touchId); // find the touch with this id
                if(touch == null) {
                    touch = unusedTouches.Dequeue(); // get a new touch from the pool
                    touches.Add(touch); // add to active touches list
                }
                touch.touchId = touchId; // store the touch id
                touch.go.SetActive(true); // show the object

                // Get the touch position
                Vector2 position;
                ds4.GetTouchPosition(i, out position);

                // Set the new position of the touch
                touch.go.transform.localPosition = new Vector3(
                    position.x - 0.5f,
                    0.5f + (touch.go.transform.localScale.y * 0.5f),
                    position.y - 0.5f
                );
            }
        }

        private class Touch {
            public GameObject go;
            public int touchId = -1;
        }
	}
}
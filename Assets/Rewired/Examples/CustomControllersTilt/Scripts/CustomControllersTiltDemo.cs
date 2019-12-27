// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.Demos {

    using UnityEngine;
    using System.Collections;
    using Rewired;
    
    /* IMPORTANT NOTE: Basic tilt control is now available through using the Tilt Control component. Please see
     * the documentation on Touch Controls for more information: http://guavaman.com/rewired/docs/TouchControls.html
     * 
     * This is a simple demo to show how to use a Custom Controller to handle tilt input on a mobile device
     */

    [AddComponentMenu("")]
    public class CustomControllersTiltDemo : MonoBehaviour {

        public Transform target; // the object that will be moving -- the cube in this case
        public float speed = 10.0F;
        private CustomController controller;
        private Player player;

        void Awake() {
            Screen.orientation = ScreenOrientation.Landscape;
            player = ReInput.players.GetPlayer(0); // get the Rewired Player
            ReInput.InputSourceUpdateEvent += OnInputUpdate; // subscribe to input update event
            controller = (CustomController)player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController"); // get the Custom Controller from the player by the Tag set in the editor
        }

        void Update() {
            if(target == null) return;

            Vector3 dir = Vector3.zero;

            // Get the tilt vectors using Action names
            dir.y = player.GetAxis("Tilt Vertical");
            dir.x = player.GetAxis("Tilt Horizontal");

            if(dir.sqrMagnitude > 1) dir.Normalize();

            dir *= Time.deltaTime;
            target.Translate(dir * speed);
        }


        /// <summary>
        /// This will be called each time input updates. Use this to push values into the Custom Controller axes.
        /// </summary>
        private void OnInputUpdate() {
            // Get the acceleration values from UnityEngine.Input and push into the controller
            Vector3 acceleration = Input.acceleration;
            controller.SetAxisValue(0, acceleration.x);
            controller.SetAxisValue(1, acceleration.y);
            controller.SetAxisValue(2, acceleration.z);
        }
    }
}
// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

using UnityEngine;
using System.Collections;

namespace Rewired.Demos {

    [AddComponentMenu("")]
    [RequireComponent(typeof(CharacterController))]
    public class PressStartToJoinExample_GamePlayer : MonoBehaviour {

        public int gamePlayerId = 0;

        public float moveSpeed = 3.0f;
        public float bulletSpeed = 15.0f;
        public GameObject bulletPrefab;

        private CharacterController cc;
        private Vector3 moveVector;
        private bool fire;

        private Rewired.Player player { get { return PressStartToJoinExample_Assigner.GetRewiredPlayer(gamePlayerId); } }

        void OnEnable() {
            // Get the character controller
            cc = GetComponent<CharacterController>();
        }

        void Update() {
            if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
            if(player == null) return;

            GetInput();
            ProcessInput();
        }

        private void GetInput() {
            // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
            // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

            moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
            moveVector.y = player.GetAxis("Move Vertical");
            fire = player.GetButtonDown("Fire");
        }

        private void ProcessInput() {
            // Process movement
            if(moveVector.x != 0.0f || moveVector.y != 0.0f) {
                cc.Move(moveVector * moveSpeed * Time.deltaTime);
            }

            // Process fire
            if(fire) {
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
            }
        }
    }
}
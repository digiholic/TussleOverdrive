// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.Demos {

    using UnityEngine;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    [RequireComponent(typeof(CharacterController))]
    public class CustomControllerDemo_Player : MonoBehaviour {

        public int playerId;
        public float speed = 1;
        public float bulletSpeed = 20;
        public GameObject bulletPrefab;

        private Player _player; // the Rewired player
        private CharacterController cc;

        private Player player {
            get {
                // Get the Rewired Player object for this player. Refresh it as needed so it will get the new reference after a script recompile in the editor.
                if(_player == null) _player = ReInput.players.GetPlayer(playerId);
                return _player;
            }
        }

        void Awake() {
            cc = GetComponent<CharacterController>();
        }

        void Update() {
            if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.

            Vector2 moveVector = new Vector2(
                player.GetAxis("Move Horizontal"),
                player.GetAxis("Move Vertical")
            );

            cc.Move(moveVector * speed * Time.deltaTime);

            if(player.GetButtonDown("Fire")) {
                Vector3 offset = Vector3.Scale(new Vector3(1, 0, 0), transform.right);
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + offset, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = new Vector3(bulletSpeed * transform.right.x, 0, 0);
            }

            if(player.GetButtonDown("Change Color")) {
                Renderer renderer = GetComponent<Renderer>();
                Material mat = renderer.material;
                mat.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
                renderer.material = mat;
            }
        }
    }
}
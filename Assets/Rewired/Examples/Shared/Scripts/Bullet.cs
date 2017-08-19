// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.Demos {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    public class Bullet : MonoBehaviour {

        public float lifeTime = 3.0f;
        private bool die;
        private float deathTime;

        void Start() {
            if(lifeTime > 0.0f) {
                deathTime = Time.time + lifeTime;
                die = true;
            }
        }

        void Update() {
            if(die && Time.time >= deathTime) Destroy(gameObject);
        }
    }
}

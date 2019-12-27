// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649 // disable warnings about unused variables

namespace Rewired.Demos.GamepadTemplateUI {

    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;
    using Rewired;

    [RequireComponent(typeof(Image))]
    public class ControllerUIEffect : MonoBehaviour {

        [SerializeField]
        private Color _highlightColor = Color.white;

        private Image _image;
        private Color _color;
        private Color _origColor;
        private bool _isActive;
        private float _highlightAmount;

        void Awake() {
            _image = GetComponent<Image>();
            _origColor = _image.color;
            _color = _origColor;
        }

        public void Activate(float amount) {
            amount = Mathf.Clamp01(amount);
            if(_isActive && amount == _highlightAmount) return; // no change to current state
            _highlightAmount = amount;
            _color = Color.Lerp(_origColor, _highlightColor, _highlightAmount);
            _isActive = true;
            RedrawImage(); // update the image
        }

        public void Deactivate() {
            if(!_isActive) return; // no change to current state
            _color = _origColor;
            _highlightAmount = 0f;
            _isActive = false;
            RedrawImage(); // update the image
        }

        void RedrawImage() {
            _image.color = _color;
            _image.enabled = _isActive;
        }
    }
}
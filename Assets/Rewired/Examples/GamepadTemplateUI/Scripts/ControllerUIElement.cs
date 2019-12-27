// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649 // disable warnings about unused variables

namespace Rewired.Demos.GamepadTemplateUI {

    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;
    using Rewired;

    [RequireComponent(typeof(Image))]
    public class ControllerUIElement : MonoBehaviour {

        [SerializeField]
        private Color _highlightColor = Color.white;
        [SerializeField]
        private ControllerUIEffect _positiveUIEffect;
        [SerializeField]
        private ControllerUIEffect _negativeUIEffect;
        [SerializeField]
        private Text _label;
        [SerializeField]
        private Text _positiveLabel;
        [SerializeField]
        private Text _negativeLabel;
        [SerializeField]
        private ControllerUIElement[] _childElements = new ControllerUIElement[0];


        private Image _image;
        private Color _color;
        private Color _origColor;
        private bool _isActive;
        private float _highlightAmount;
        
        private bool hasEffects { get { return _positiveUIEffect != null || _negativeUIEffect != null; } }

        void Awake() {
            _image = GetComponent<Image>();
            _origColor = _image.color;
            _color = _origColor;
            ClearLabels();
        }

        public void Activate(float amount) {
            amount = Mathf.Clamp(amount, -1f, 1f);

            if(hasEffects) {
                // Effects exist so activate each as appropriate
                if(amount < 0 && _negativeUIEffect != null) _negativeUIEffect.Activate(Mathf.Abs(amount));
                if(amount > 0 && _positiveUIEffect != null) _positiveUIEffect.Activate(Mathf.Abs(amount));
            } else {
                // No effects so just highlight this Image
                if(_isActive && amount == _highlightAmount) return; // no change to current state
                _highlightAmount = amount;
                _color = Color.Lerp(_origColor, _highlightColor, _highlightAmount);
            }

            _isActive = true;
            RedrawImage(); // update the image

            // Report to child elements
            if(_childElements.Length != 0) {
                for(int i = 0; i < _childElements.Length; i++) {
                    if(_childElements[i] == null) continue;
                    _childElements[i].Activate(amount);
                }
            }
        }

        public void Deactivate() {
            if(!_isActive) return; // no change to current state
            _color = _origColor;
            _highlightAmount = 0f;
            // Deactivate the positive and negative effects
            if(_positiveUIEffect != null) _positiveUIEffect.Deactivate();
            if(_negativeUIEffect != null) _negativeUIEffect.Deactivate();
            _isActive = false;
            RedrawImage(); // update the image

            // Report to child elements
            if(_childElements.Length != 0) {
                for(int i = 0; i < _childElements.Length; i++) {
                    if(_childElements[i] == null) continue;
                    _childElements[i].Deactivate();
                }
            }
        }

        public void SetLabel(string text, AxisRange labelType) {
            Text label;
            switch(labelType) {
                case AxisRange.Full:
                    label = _label;
                    break;
                case AxisRange.Positive:
                    label = _positiveLabel;
                    break;
                case AxisRange.Negative:
                    label = _negativeLabel;
                    break;
                default:
                    label = null;
                    break;
            }
            if(label != null) {
                label.text = text;
            }

            // Report to child elements
            if(_childElements.Length != 0) {
                for(int i = 0; i < _childElements.Length; i++) {
                    if(_childElements[i] == null) continue;
                    _childElements[i].SetLabel(text, labelType);
                }
            }
        }

        public void ClearLabels() {
            if(_label != null) _label.text = string.Empty;
            if(_positiveLabel != null) _positiveLabel.text = string.Empty;
            if(_negativeLabel != null) _negativeLabel.text = string.Empty;
            // Report to child elements
            if(_childElements.Length != 0) {
                for(int i = 0; i < _childElements.Length; i++) {
                    if(_childElements[i] == null) continue;
                    _childElements[i].ClearLabels();
                }
            }
        }

        void RedrawImage() {
            _image.color = _color;
        }
    }
}
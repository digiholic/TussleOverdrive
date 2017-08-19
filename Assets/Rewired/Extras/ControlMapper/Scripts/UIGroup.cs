// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649

namespace Rewired.UI.ControlMapper {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Collections;
    using Rewired;

    [AddComponentMenu("")]
    public class UIGroup : MonoBehaviour {

        [SerializeField]
        private Text _label;
        [SerializeField]
        private Transform _content;

        public string labelText {
            get {
                return _label != null ? _label.text : string.Empty;
            }
            set {
                if(_label == null) return;
                _label.text = value;
            }
        }

        public Transform content { get { return _content; } }

        public void SetLabelActive(bool state) {
            if(_label == null) return;
            _label.gameObject.SetActive(state);
        }
    }
}

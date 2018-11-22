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
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// A simple pointer for Unity UI.
    /// </summary>
    [AddComponentMenu("")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIPointer : UIBehaviour {

        [Tooltip("Should the hardware pointer be hidden?")]
        [SerializeField]
        private bool _hideHardwarePointer = true;

        [Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
        [SerializeField]
        private bool _autoSort = true;

        [NonSerialized]
        private RectTransform _canvasRectTransform;

        /// <summary>
        /// Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.
        /// </summary>
        public bool autoSort { get { return _autoSort; } set { if(value == _autoSort) return; _autoSort = value; if(value) transform.SetAsLastSibling(); } }

        protected override void Awake() {
            base.Awake();

#if UNITY_5_2_PLUS
            // Disable raycasting on all Graphics in the pointer
            Graphic[] graphics = GetComponentsInChildren<Graphic>();
            foreach(Graphic g in graphics) {
                g.raycastTarget = false;
            }
#endif
#if UNITY_5_PLUS
            // Hide the hardware pointer
            if(_hideHardwarePointer) Cursor.visible = false;
#endif
            if(_autoSort) transform.SetAsLastSibling();

            GetDependencies();
        }

        private void Update() {
            if(_autoSort && transform.GetSiblingIndex() < transform.parent.childCount - 1) {
                transform.SetAsLastSibling();
            }
        }

        protected override void OnTransformParentChanged() {
            base.OnTransformParentChanged();
            GetDependencies();
        }

        protected override void OnCanvasGroupChanged() {
 	        base.OnCanvasGroupChanged();
            GetDependencies();
        }

        /// <summary>
        /// Updates the pointer position.
        /// </summary>
        /// <param name="screenPosition">The screen position of the pointer.</param>
        public void OnScreenPositionChanged(Vector2 screenPosition) {
            if(_canvasRectTransform == null) return;
            
            Rect rootCanvasRect = _canvasRectTransform.rect;
            Vector2 viewportPos = Camera.main.ScreenToViewportPoint(screenPosition);

            viewportPos.x = (viewportPos.x * rootCanvasRect.width) - _canvasRectTransform.pivot.x * rootCanvasRect.width;
            viewportPos.y = (viewportPos.y * rootCanvasRect.height) - _canvasRectTransform.pivot.y * rootCanvasRect.height;
            (transform as RectTransform).anchoredPosition = viewportPos;
        }

        private void GetDependencies() {
            Canvas canvas = transform.root.GetComponentInChildren<Canvas>();
            _canvasRectTransform = canvas != null ? canvas.GetComponent<RectTransform>() : null;
        }
    }
}
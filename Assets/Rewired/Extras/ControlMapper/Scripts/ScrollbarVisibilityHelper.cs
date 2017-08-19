// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649

namespace Rewired.UI.ControlMapper {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Rewired;

    /// <summary>
    /// Hides ScrollRect scrollbars based on the dimensions of the content.
    /// This must be placed on the ScrollRect.content GameObject.
    /// </summary>
    [AddComponentMenu("")]
    public class ScrollbarVisibilityHelper : MonoBehaviour {

        public ScrollRect scrollRect;

        private Scrollbar hScrollBar { get { return scrollRect != null ? scrollRect.horizontalScrollbar : null; } }
        private Scrollbar vScrollBar { get { return scrollRect != null ? scrollRect.verticalScrollbar : null; } }

        // Used by component on ScrollRect that is used just for sending messages when its size changes
        private bool onlySendMessage;
        private ScrollbarVisibilityHelper target;

        void Awake() {
            // Add component on parent ScrollRect so we know when its dimensions change too
            if(scrollRect != null) {
                target = scrollRect.gameObject.AddComponent<ScrollbarVisibilityHelper>();
                target.onlySendMessage = true;
                target.target = this;
            }
        }

        void OnRectTransformDimensionsChange() {
            if(onlySendMessage) { // this is a parent ScrollRect, just send a message to target
                if(target != null) target.ScrollRectTransformDimensionsChanged();
            } else { // this is the component on the content game object, evaluate the scroll bars
                EvaluateScrollbar();
            }
        }

        private void ScrollRectTransformDimensionsChanged() {
            OnRectTransformDimensionsChange();
        }

        private void EvaluateScrollbar() {
            if(scrollRect == null) return;
            if(vScrollBar == null && hScrollBar == null) return;
            if(!gameObject.activeInHierarchy) return; // exit if not enabled or coroutine will fail

            Rect contentRect = scrollRect.content.rect;
            Rect visibleRect = (scrollRect.transform as RectTransform).rect;

            if(vScrollBar != null) {
                bool newState = contentRect.height <= visibleRect.height ? false : true;
                SetActiveDeferred(vScrollBar.gameObject, newState);
            }

            if(hScrollBar != null) {
                bool newState = contentRect.width <= visibleRect.width ? false : true;
                SetActiveDeferred(hScrollBar.gameObject, newState);
            }
        }

        private void SetActiveDeferred(GameObject obj, bool value) {
            StopAllCoroutines(); // clear all coroutines in case any were still pending -- this gets around an issue where the UI is being built and the enabled state runs several times in a row and coroutines playback in opposite order

            // Sets the active state after 1 frame because UI is being rebuilt when OnRectTransformDimensionsChange takes place and not allowed to set active at this time.
            StartCoroutine(SetActiveCoroutine(obj, value));
        }

        private IEnumerator SetActiveCoroutine(GameObject obj, bool value) {
            yield return null;
            if(obj != null) {
                obj.SetActive(value);
            }
        }

    }
}
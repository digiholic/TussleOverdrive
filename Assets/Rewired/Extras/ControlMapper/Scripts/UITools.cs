// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649

namespace Rewired.UI.ControlMapper {

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Rewired;

    public static class UITools {

        public static GameObject InstantiateGUIObject<T>(GameObject prefab, Transform parent, string name) where T : Component {
            GameObject instance = InstantiateGUIObject_Pre<T>(prefab, parent, name);
            if(instance == null) return null;

            RectTransform rt = instance.GetComponent<RectTransform>();
            if(rt == null) {
                Debug.LogError(name + " prefab is missing RectTransform component!");
            } else {
                rt.localScale = Vector3.one;
            }
            return instance;
        }
        public static GameObject InstantiateGUIObject<T>(GameObject prefab, Transform parent, string name, Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition) where T : Component {
            GameObject instance = InstantiateGUIObject_Pre<T>(prefab, parent, name);
            if(instance == null) return null;

            RectTransform rt = instance.GetComponent<RectTransform>();
            if(rt == null) {
                Debug.LogError(name + " prefab is missing RectTransform component!");
            } else {
                rt.localScale = Vector3.one;
                rt.pivot = pivot;
                rt.anchorMin = anchorMin;
                rt.anchorMax = anchorMax;
                rt.anchoredPosition = anchoredPosition;
            }
            return instance;
        }
        private static GameObject InstantiateGUIObject_Pre<T>(GameObject prefab, Transform parent, string name) where T : Component {
            if(prefab == null) {
                Debug.LogError(name + " prefab is null!");
                return null;
            }
            GameObject instance = (GameObject)Object.Instantiate(prefab);
            if(!string.IsNullOrEmpty(name)) instance.name = name;
            T comp = instance.GetComponent<T>();
            if(comp == null) {
                Debug.LogError(name + " prefab is missing the " + comp.GetType().ToString() + " component!");
                return null;
            }
            if(parent != null) {
                instance.transform.SetParent(parent, false);
            }
            return instance;
        }

        public static Vector3 GetPointOnRectEdge(RectTransform rectTransform, Vector2 dir) {
            if(rectTransform == null)
                return Vector3.zero;
            if(dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            Rect rect = rectTransform.rect;
            dir = rect.center + Vector2.Scale(rect.size, dir * 0.5f);
            return dir;
        }

        public static Rect GetWorldSpaceRect(RectTransform rt) {
            if(rt == null) return new Rect();
            Rect rect = rt.rect;

            // Create a new rect in world coords
            Vector3 tl = rt.TransformPoint(new Vector2(rect.xMin, rect.yMin)); // top left
            Vector3 bl = rt.TransformPoint(new Vector2(rect.xMin, rect.yMax)); // bottom left
            Vector3 tr = rt.TransformPoint(new Vector2(rect.xMax, rect.yMin)); // top right

            // Resulting Rect is in 3D coords, NOT Unity Rect coords.
            // IE: 0 Y = bottom left, + values going UP
            return new Rect(tl.x, tl.y, tr.x - tl.x, bl.y - tl.y); // invert y
        }

        public static void SetInteractable(Selectable selectable, bool state, bool playTransition) {
            if(selectable == null) return;

            if(!playTransition) {
                // Workaround because Unity will not set the state of an element immediately if a fade is present. This causes numerous graphical issues.
                if(selectable.transition == Selectable.Transition.ColorTint) {
                    ColorBlock colorBlock = selectable.colors;
                    float prevFadeDuration = colorBlock.fadeDuration;
                    colorBlock.fadeDuration = 0;
                    selectable.colors = colorBlock;
                    selectable.interactable = state;
                    colorBlock.fadeDuration = prevFadeDuration;
                    selectable.colors = colorBlock;
                }
            } else {
                selectable.interactable = state;
            }
        }
    }
}
// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.
#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649

namespace Rewired.UI.ControlMapper {

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using System.Collections;
    using Rewired;

    public static class UISelectionUtility {

        // Find the next selectable object in the specified world-space direction.
        public static Selectable FindNextSelectable(Selectable selectable, Transform transform, List<Selectable> allSelectables, Vector3 direction) {
            RectTransform rectTransform = transform as RectTransform;
            if(rectTransform == null) return null;
            
            direction = direction.normalized;
            Vector2 localDir = Quaternion.Inverse(transform.rotation) * direction;
            Vector2 searchStartPos = transform.TransformPoint(Rewired.UI.ControlMapper.UITools.GetPointOnRectEdge(rectTransform, localDir)); // search from point on rect edge from center out in direction
            //Vector2 searchStartPos = transform.TransformPoint((transform as RectTransform).rect.center); // search from center
            bool isHoriz = direction == Vector3.left || direction == Vector3.right;

            float minCenterDistSqMag = Mathf.Infinity;
            float minDirectLineSqMag = Mathf.Infinity;
            Selectable bestCenterDistPick = null;
            Selectable bestDirectLinePick = null;

            const float length = 999999f; // Mathf.Infinity fails
            Vector2 directLineCastEndPos = searchStartPos + localDir * length;

            for(int i = 0; i < allSelectables.Count; ++i) {
                Selectable sel = allSelectables[i];

                if(sel == selectable || sel == null) continue; // skip if self or null
                if(sel.navigation.mode == Navigation.Mode.None) continue; // skip if non-navigable

                // Allow selection of non-interactable elements because it makes navigating easier and more predictable
                // but the CanvasGroup interactable value is private in Selectable
#if !UNITY_WSA
                // Reflect to get group intaractability if non-interactable
                bool canvasGroupAllowsInteraction = sel.IsInteractable() || Rewired.Utils.ReflectionTools.GetPrivateField<Selectable, bool>(sel, "m_GroupsAllowInteraction");
                if(!canvasGroupAllowsInteraction) continue; // skip if disabled by canvas group, otherwise allow it
#else
                // Can't do private field reflection in Metro
                if(!sel.IsInteractable()) continue; // skip if disabled
#endif

                var selRect = sel.transform as RectTransform;
                if(selRect == null) continue;

                // Check direct line cast from center edge of object in direction pressed
                float directLineSqMag;
                Rect worldSpaceRect = Rewired.UI.ControlMapper.UITools.GetWorldSpaceRect(selRect);

                // Check for direct line rect intersection
                if(Rewired.Utils.MathTools.LineIntersectsRect(searchStartPos, directLineCastEndPos, worldSpaceRect, out directLineSqMag)) {
                    if(isHoriz) directLineSqMag *= 0.25f; // give extra bonus to horizontal directions because most of the UI groups are laid out horizontally                     
                    if(directLineSqMag < minDirectLineSqMag) {
                        minDirectLineSqMag = directLineSqMag;
                        bestDirectLinePick = sel;
                    }
                }

                // Check distance to center
                Vector2 selCenter = (Vector3)selRect.rect.center;
                Vector2 searchPosToSelCenter = (Vector2)sel.transform.TransformPoint(selCenter) - searchStartPos;

                const float maxSafeAngle = 75.0f;

                // Get the angle the target center deviates from straight
                float angle = Mathf.Abs(Vector2.Angle(localDir, searchPosToSelCenter));
                if(angle > maxSafeAngle) continue; // only consider if within a reasonable angle of the desired direction

                float score = searchPosToSelCenter.sqrMagnitude;

                // Lower score is better
                if(score < minCenterDistSqMag) {
                    minCenterDistSqMag = score;
                    bestCenterDistPick = sel;
                }
            }

            // Choose between direct line and center dist
            if(bestDirectLinePick != null && bestCenterDistPick != null) {
                if(minDirectLineSqMag > minCenterDistSqMag) {
                    return bestCenterDistPick;
                }
                return bestDirectLinePick;
            }

            return bestDirectLinePick ?? bestCenterDistPick;
        }
    }
}

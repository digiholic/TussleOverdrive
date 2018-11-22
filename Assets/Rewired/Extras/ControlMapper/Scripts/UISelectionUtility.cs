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

            direction.Normalize();

            Vector2 localDir = direction;
            Vector2 searchStartPos = Rewired.UI.ControlMapper.UITools.GetPointOnRectEdge(rectTransform, localDir); // search from point on rect edge from center out in direction
            bool isHoriz = localDir == Vector2.right * -1f || localDir == Vector2.right;

            float minCenterDistSqMag = Mathf.Infinity;
            float minDirectLineSqMag = Mathf.Infinity;
            Selectable bestCenterDistPick = null;
            Selectable bestDirectLinePick = null;

            const float length = 999999f; // Mathf.Infinity fails
            Vector2 directLineCastEndPos = searchStartPos + localDir * length;

            for(int i = 0; i < allSelectables.Count; ++i) {
                Selectable targetSelectable = allSelectables[i];

                if(targetSelectable == selectable || targetSelectable == null) continue; // skip if self or null
                if(targetSelectable.navigation.mode == Navigation.Mode.None) continue; // skip if non-navigable

                // Allow selection of non-interactable elements because it makes navigating easier and more predictable
                // but the CanvasGroup interactable value is private in Selectable
#if !UNITY_WSA
                // Reflect to get group intaractability if non-interactable
                bool canvasGroupAllowsInteraction = targetSelectable.IsInteractable() || Rewired.Utils.ReflectionTools.GetPrivateField<Selectable, bool>(targetSelectable, "m_GroupsAllowInteraction");
                if(!canvasGroupAllowsInteraction) continue; // skip if disabled by canvas group, otherwise allow it
#else
                // Can't do private field reflection in Metro
                if(!targetSelectable.IsInteractable()) continue; // skip if disabled
#endif

                var targetSelectableRectTransform = targetSelectable.transform as RectTransform;
                if(targetSelectableRectTransform == null) continue;

                // Check direct line cast from center edge of object in direction pressed
                float directLineSqMag;
                Rect targetSelecableRect = UITools.InvertY(UITools.TransformRectTo(targetSelectableRectTransform, transform, targetSelectableRectTransform.rect));

                // Check for direct line rect intersection
                if(Rewired.Utils.MathTools.LineIntersectsRect(searchStartPos, directLineCastEndPos, targetSelecableRect, out directLineSqMag)) {
                    if(isHoriz) directLineSqMag *= 0.25f; // give extra bonus to horizontal directions because most of the UI groups are laid out horizontally                     
                    if(directLineSqMag < minDirectLineSqMag) {
                        minDirectLineSqMag = directLineSqMag;
                        bestDirectLinePick = targetSelectable;
                    }
                }

                // Check distance to center
                Vector2 targetSelectableCenter = Rewired.Utils.UnityTools.TransformPoint(targetSelectableRectTransform, transform, targetSelectableRectTransform.rect.center);
                Vector2 searchPosToTargetSelectableCenter = targetSelectableCenter - searchStartPos;

                const float maxSafeAngle = 75.0f;

                // Get the angle the target center deviates from straight
                float angle = Mathf.Abs(Vector2.Angle(localDir, searchPosToTargetSelectableCenter));

                if(angle > maxSafeAngle) continue; // only consider if within a reasonable angle of the desired direction

                float score = searchPosToTargetSelectableCenter.sqrMagnitude;

                // Lower score is better
                if(score < minCenterDistSqMag) {
                    minCenterDistSqMag = score;
                    bestCenterDistPick = targetSelectable;
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

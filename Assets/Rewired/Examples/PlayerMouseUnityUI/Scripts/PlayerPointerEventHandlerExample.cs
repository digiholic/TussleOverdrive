// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0219
#pragma warning disable 0618
#pragma warning disable 0649

namespace Rewired.Demos {
    using System.Text;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Rewired.Integration.UnityUI;

    /// <summary>
    /// Example handler of Player Pointer Events.
    /// </summary>
    [AddComponentMenu("")]
    public sealed class PlayerPointerEventHandlerExample : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler, 
        IPointerUpHandler, 
        IPointerDownHandler, 
        IPointerClickHandler,
        IScrollHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {

        public Text text;
        private const int logLength = 10;
        private List<string> log = new List<string>();

        private void Log(string o) {
            log.Add(o);
            if(log.Count > logLength) log.RemoveAt(0);
        }

        void Update() {
            if(text != null) {
                StringBuilder sb = new StringBuilder();
                foreach(var s in log) {
                    sb.AppendLine(s);
                }
                text.text = sb.ToString();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnPointerEnter: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData));
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnPointerExit: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData));
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
 	        if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnPointerUp: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
 	        if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnPointerDown: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
 	        if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnPointerClick: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }
    
        public void OnScroll(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnScroll: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData));
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnBeginDrag: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnDrag: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if(eventData is PlayerPointerEventData) {
                PlayerPointerEventData playerEventData = (PlayerPointerEventData)eventData;
                Log("OnEndDrag: " + " Player = " + playerEventData.playerId + ", Pointer Index = " + playerEventData.inputSourceIndex + ", Source = " + GetSourceName(playerEventData) + ", Button Index = " + playerEventData.buttonIndex);
            }
        }

        private static string GetSourceName(PlayerPointerEventData playerEventData) {
            if(playerEventData.sourceType == PointerEventType.Mouse) {
                if(playerEventData.mouseSource is Behaviour) return (playerEventData.mouseSource as Behaviour).name;
            } else if(playerEventData.sourceType == PointerEventType.Touch) {
                if(playerEventData.touchSource is Behaviour) return (playerEventData.touchSource as Behaviour).name;
            }
            return null;
        }
    }
}
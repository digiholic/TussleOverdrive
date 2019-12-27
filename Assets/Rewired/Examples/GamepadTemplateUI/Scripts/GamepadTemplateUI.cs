// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649 // disable warnings about unused variables

/* This example shows how determine which Controller Template
 * elements correspond to mapped Actions in a Player, both those
 * that are currently active and those that are mapped.
 * These concepts be used to display a Controller Template based mapping
 * UI, display Controller Template element names / glyphs instead
 * of Controller-specific element names / glyphs, etc.
 * The same concepts can be used to display help UI using
 * Controller Template elements instead of Controller-specific
 * elements.
 * 
 * This example is NOT meant to be modified and used in your game.
 * It is only intended to teach the API.
 */

namespace Rewired.Demos.GamepadTemplateUI {
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;

    public class GamepadTemplateUI : MonoBehaviour {

        private const float stickRadius = 20f;

        public int playerId = 0;

        [SerializeField]
        private RectTransform leftStick;
        [SerializeField]
        private RectTransform rightStick;

        [SerializeField]
        private ControllerUIElement leftStickX;
        [SerializeField]
        private ControllerUIElement leftStickY;
        [SerializeField]
        private ControllerUIElement leftStickButton;
        [SerializeField]
        private ControllerUIElement rightStickX;
        [SerializeField]
        private ControllerUIElement rightStickY;
        [SerializeField]
        private ControllerUIElement rightStickButton;
        [SerializeField]
        private ControllerUIElement actionBottomRow1;
        [SerializeField]
        private ControllerUIElement actionBottomRow2;
        [SerializeField]
        private ControllerUIElement actionBottomRow3;
        [SerializeField]
        private ControllerUIElement actionTopRow1;
        [SerializeField]
        private ControllerUIElement actionTopRow2;
        [SerializeField]
        private ControllerUIElement actionTopRow3;
        [SerializeField]
        private ControllerUIElement leftShoulder;
        [SerializeField]
        private ControllerUIElement leftTrigger;
        [SerializeField]
        private ControllerUIElement rightShoulder;
        [SerializeField]
        private ControllerUIElement rightTrigger;
        [SerializeField]
        private ControllerUIElement center1;
        [SerializeField]
        private ControllerUIElement center2;
        [SerializeField]
        private ControllerUIElement center3;
        [SerializeField]
        private ControllerUIElement dPadUp;
        [SerializeField]
        private ControllerUIElement dPadRight;
        [SerializeField]
        private ControllerUIElement dPadDown;
        [SerializeField]
        private ControllerUIElement dPadLeft;

#if !REWIRED_USE_USER_DEFINED_CONTROLLER_TEMPLATES

        private UIElement[] _uiElementsArray;
        private Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();
        private IList<ControllerTemplateElementTarget> _tempTargetList = new List<ControllerTemplateElementTarget>(2);
        private Stick[] _sticks;

        private Player player { get { return ReInput.players.GetPlayer(playerId); } }

        private void Awake() {

            // Create an array of all UI elements so we can iterate this later
            _uiElementsArray = new UIElement[] {
                new UIElement(GamepadTemplate.elementId_leftStickX, leftStickX),
                new UIElement(GamepadTemplate.elementId_leftStickY, leftStickY),
                new UIElement(GamepadTemplate.elementId_leftStickButton, leftStickButton),
                new UIElement(GamepadTemplate.elementId_rightStickX, rightStickX),
                new UIElement(GamepadTemplate.elementId_rightStickY, rightStickY),
                new UIElement(GamepadTemplate.elementId_rightStickButton, rightStickButton),
                new UIElement(GamepadTemplate.elementId_actionBottomRow1, actionBottomRow1),
                new UIElement(GamepadTemplate.elementId_actionBottomRow2, actionBottomRow2),
                new UIElement(GamepadTemplate.elementId_actionBottomRow3, actionBottomRow3),
                new UIElement(GamepadTemplate.elementId_actionTopRow1, actionTopRow1),
                new UIElement(GamepadTemplate.elementId_actionTopRow2, actionTopRow2),
                new UIElement(GamepadTemplate.elementId_actionTopRow3, actionTopRow3),
                new UIElement(GamepadTemplate.elementId_center1, center1),
                new UIElement(GamepadTemplate.elementId_center2, center2),
                new UIElement(GamepadTemplate.elementId_center3, center3),
                new UIElement(GamepadTemplate.elementId_dPadUp, dPadUp),
                new UIElement(GamepadTemplate.elementId_dPadRight, dPadRight),
                new UIElement(GamepadTemplate.elementId_dPadDown, dPadDown),
                new UIElement(GamepadTemplate.elementId_dPadLeft, dPadLeft),
                new UIElement(GamepadTemplate.elementId_leftShoulder1, leftShoulder),
                new UIElement(GamepadTemplate.elementId_leftShoulder2, leftTrigger),
                new UIElement(GamepadTemplate.elementId_rightShoulder1, rightShoulder),
                new UIElement(GamepadTemplate.elementId_rightShoulder2, rightTrigger)
            };

            // Add UI elements to the dictionary so we can look them up by Element Identifier Id
            for(int i = 0; i < _uiElementsArray.Length; i++) {
                _uiElements.Add(_uiElementsArray[i].id, _uiElementsArray[i].element);
            }

            // Create Sticks which will be used to move the sticks on the UI
            _sticks = new Stick[] {
                new Stick(leftStick, GamepadTemplate.elementId_leftStickX, GamepadTemplate.elementId_leftStickY),
                new Stick(rightStick, GamepadTemplate.elementId_rightStickX, GamepadTemplate.elementId_rightStickY)
            };

            // Subscribe to controller connection events
            ReInput.ControllerConnectedEvent += OnControllerConnected;
            ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        }

        private void Start() {
            if(!ReInput.isReady) return;
            DrawLabels();
        }

        private void OnDestroy() {
            // Unsubscribe from events
            ReInput.ControllerConnectedEvent -= OnControllerConnected;
            ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
        }

        private void Update() {
            if(!ReInput.isReady) return;
            DrawActiveElements();
        }

        private void DrawActiveElements() {

            // Deactivate all elements first
            for(int i = 0; i < _uiElementsArray.Length; i++) {
                _uiElementsArray[i].element.Deactivate();
            }

            // Reset stick positions
            for(int i = 0; i < _sticks.Length; i++) {
                _sticks[i].Reset();
            }

            // Activate elements currently active based on the Action
            IList<InputAction> actions = ReInput.mapping.Actions;
            for(int i = 0; i < actions.Count; i++) {
                ActivateElements(player, actions[i].id);
            }
        }

        private void ActivateElements(Player player, int actionId) {

            // Get the Axis value of the Action from the Player
            float axisValue = player.GetAxis(actionId);
            if(axisValue == 0f) return; // not active

            // Get all the sources contributing to the Action
            IList<InputActionSourceData> sources = player.GetCurrentInputSources(actionId);

            // Check each source and activate the elements they map to
            for(int i = 0; i < sources.Count; i++) {
                InputActionSourceData source = sources[i];

                // Try to get the GamepadTemplate from the Controller
                IGamepadTemplate gamepad = source.controller.GetTemplate<IGamepadTemplate>();
                if(gamepad == null) continue; // does not implement the Dual Analog Gamepad Template

                // Get all element targets on the template by passing in the Action Element Map
                // This gives you the Template element targets that are mapped to the Controller element target.
                gamepad.GetElementTargets(source.actionElementMap, _tempTargetList);

                // Activate Template element targets
                for(int j = 0; j < _tempTargetList.Count; j++) {
                    ControllerTemplateElementTarget target = _tempTargetList[j];
                    int templateElementId = target.element.id;
                    ControllerUIElement uiElement = _uiElements[templateElementId];

                    if(target.elementType == ControllerTemplateElementType.Axis) {
                        uiElement.Activate(axisValue);
                    } else if(target.elementType == ControllerTemplateElementType.Button) {
                        // If the target element is a Button, make sure the Button value of the Action is true before activating
                        if(player.GetButton(actionId) || player.GetNegativeButton(actionId)) uiElement.Activate(1f);
                    }

                    // Move the stick for stick axes
                    Stick stick = GetStick(templateElementId);
                    if(stick != null) stick.SetAxisPosition(templateElementId, axisValue * stickRadius);
                }
            }
        }

        private void DrawLabels() {
            // Clear UI labels first
            for(int i = 0; i < _uiElementsArray.Length; i++) {
                _uiElementsArray[i].element.ClearLabels();
            }

            // Draw the label for each Action
            IList<InputAction> actions = ReInput.mapping.Actions;
            for(int i = 0; i < actions.Count; i++) {
                DrawLabels(player, actions[i]);
            }
        }

        private void DrawLabels(Player player, InputAction action) {

            // Lists first Action bound to each Dual Analog Gamepad Template element

            // Get the first Controller that implements the Dual Analog Gamepad Template from the Player
            Controller controller = player.controllers.GetFirstControllerWithTemplate<IGamepadTemplate>();
            if(controller == null) return;

            // Get the first gamepad assigned to the Player
            IGamepadTemplate gamepad = controller.GetTemplate<IGamepadTemplate>();

            // Get the Controller Map in the Default category and layout for this Controller from the Player
            ControllerMap controllerMap = player.controllers.maps.GetMap(controller, "Default", "Default");
            if(controllerMap == null) return;

            // Go through each Controller Template element displayed in the UI
            for(int i = 0; i < _uiElementsArray.Length; i++) {
                ControllerUIElement uiElement = _uiElementsArray[i].element;
                int elementId = _uiElementsArray[i].id;

                // Get the Controller Template Element from the Controller Template
                IControllerTemplateElement element = gamepad.GetElement(elementId);

                // Draw the label
                DrawLabel(uiElement, action, controllerMap, gamepad, element);
            }
        }

        private void DrawLabel(ControllerUIElement uiElement, InputAction action, ControllerMap controllerMap, IControllerTemplate template, IControllerTemplateElement element) {
            if(element.source == null) return; // this element cannot map to a source

            ActionElementMap aem;

            // A Controller Template Element Source contains Targets that each point to
            // a Controller.Element or part of a Controller.Element (such as one pole of an Axis).

            // Handle Axis-type Template Element
            if(element.source.type == ControllerTemplateElementSourceType.Axis) {
                
                // Cast the source to an Axis source so we can access the 3 possible Targets
                IControllerTemplateAxisSource source = (element.source as IControllerTemplateAxisSource);

                // A Template axis source can be either a full-axis binding or a split-axis binding

                // Handle split-axis source
                if(source.splitAxis) {

                    // A split-axis source has a Positive Target and a Negative Target, one for each side of the axis.

                    // Positive Target
                    aem = controllerMap.GetFirstElementMapWithElementTarget(source.positiveTarget, action.id, true);
                    if(aem != null) {
                        uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Positive);
                    }

                    // Negative Target
                    aem = controllerMap.GetFirstElementMapWithElementTarget(source.negativeTarget, action.id, true);
                    if(aem != null) uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Negative);

                // Handle full-axis source
                } else {

                    // A full-axis sources has just a single full target.

                    // Full Target
                    aem = controllerMap.GetFirstElementMapWithElementTarget(source.fullTarget, action.id, true);
                    if(aem != null) { // a full target was mapped

                        uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Full);

                    } else { // no full mapping was found, look for separate positive/negative mappings

                        // Positive side
                        aem = controllerMap.GetFirstElementMapWithElementTarget(
                            new ControllerElementTarget(source.fullTarget) { axisRange = AxisRange.Positive },
                            action.id,
                            true
                        );
                        if(aem != null) uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Positive);

                        // Negative side
                        aem = controllerMap.GetFirstElementMapWithElementTarget(
                            new ControllerElementTarget(source.fullTarget) { axisRange = AxisRange.Negative },
                            action.id,
                            true
                        );
                        if(aem != null) uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Negative);
                    }
                }

            // Handle Button-type Template Element
            } else if(element.source.type == ControllerTemplateElementSourceType.Button) {

                // Cast the source to an button source
                IControllerTemplateButtonSource source = (element.source as IControllerTemplateButtonSource);

                // Target
                aem = controllerMap.GetFirstElementMapWithElementTarget(source.target, action.id, true);
                if(aem != null) uiElement.SetLabel(aem.actionDescriptiveName, AxisRange.Full);
            }
        }

        private Stick GetStick(int elementId) {
            for(int i = 0; i < _sticks.Length; i++) {
                if(_sticks[i].ContainsElement(elementId)) return _sticks[i];
            }
            return null;
        }

        private void OnControllerConnected(ControllerStatusChangedEventArgs args) {
            DrawLabels();
        }

        private void OnControllerDisconnected(ControllerStatusChangedEventArgs args) {
            DrawLabels();
        }

        private class Stick {

            private RectTransform _transform;
            private Vector2 _origPosition;
            private int _xAxisElementId = -1;
            private int _yAxisElementId = -1;

            public Vector2 position {
                get {
                    return _transform != null ? _transform.anchoredPosition - _origPosition : Vector2.zero;
                }
                set {
                    if(_transform == null) return;
                    _transform.anchoredPosition = _origPosition + value;
                }
            }

            public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId) {
                if(transform == null) return;
                _transform = transform;
                _origPosition = _transform.anchoredPosition;
                _xAxisElementId = xAxisElementId;
                _yAxisElementId = yAxisElementId;
            }

            public void Reset() {
                if(_transform == null) return;
                _transform.anchoredPosition = _origPosition;
            }

            public bool ContainsElement(int elementId) {
                if(_transform == null) return false;
                return elementId == _xAxisElementId || elementId == _yAxisElementId;
            }

            public void SetAxisPosition(int elementId, float value) {
                if(_transform == null) return;
                Vector2 position = this.position;
                if(elementId == _xAxisElementId) position.x = value;
                else if(elementId == _yAxisElementId) position.y = value;
                this.position = position;
            }
        }

        private class UIElement {
            public int id;
            public ControllerUIElement element;

            public UIElement(int id, ControllerUIElement element) {
                this.id = id;
                this.element = element;
            }
        }
#endif
    }
}
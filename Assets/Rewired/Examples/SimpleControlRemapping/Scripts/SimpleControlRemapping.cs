// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

/* This is a very basic example of control remapping using the InputMapper class.
 * This example only supports 1 Player, 1 Joystick, 1 Controller Map, and 1 mapping
 * per Action, per controller type. No UI windows are used and conflict checking
 * is handled automatically by InputMapper to keep this example small and focused.
 */

namespace Rewired.Demos {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;

    [AddComponentMenu("")]
    public class SimpleControlRemapping : MonoBehaviour {

        private const string category = "Default";
        private const string layout = "Default";

        private InputMapper inputMapper = new InputMapper();

        public GameObject buttonPrefab;
        public GameObject textPrefab;
        public RectTransform fieldGroupTransform;
        public RectTransform actionGroupTransform;
        public Text controllerNameUIText;
        public Text statusUIText;

        private ControllerType selectedControllerType = ControllerType.Keyboard;
        private int selectedControllerId = 0;
        private List<Row> rows = new List<Row>();

        private Player player { get { return ReInput.players.GetPlayer(0); } }
        private ControllerMap controllerMap {
            get {
                if(controller == null) return null;
                return player.controllers.maps.GetMap(controller.type, controller.id, category, layout);
            }
        }
        private Controller controller { get { return player.controllers.GetController(selectedControllerType, selectedControllerId); } }

        private void OnEnable() {
            if(!ReInput.isReady) return; // don't run if Rewired hasn't been initialized

            // Timeout after 5 seconds of listening
            inputMapper.options.timeout = 5f;

            // Ignore Mouse X and Y axes
            inputMapper.options.ignoreMouseXAxis = true;
            inputMapper.options.ignoreMouseYAxis = true;
            
            // Subscribe to events
            ReInput.ControllerConnectedEvent += OnControllerChanged;
            ReInput.ControllerDisconnectedEvent += OnControllerChanged;
            inputMapper.InputMappedEvent += OnInputMapped;
            inputMapper.StoppedEvent += OnStopped;

            // Create UI elements
            InitializeUI();
        }

        private void OnDisable() {

            // Make sure the input mapper is stopped first
            inputMapper.Stop();

            // Unsubscribe from events
            inputMapper.RemoveAllEventListeners();
            ReInput.ControllerConnectedEvent -= OnControllerChanged;
            ReInput.ControllerDisconnectedEvent -= OnControllerChanged;
        }

        private void RedrawUI() {
            if(controller == null) { // no controller is selected
                ClearUI();
                return;
            }

            // Update joystick name in UI
            controllerNameUIText.text = controller.name;

            // Update each button label with the currently mapped element identifier
            for(int i = 0; i < rows.Count; i++) {
                Row row = rows[i];
                InputAction action = rows[i].action;

                string name = string.Empty;
                int actionElementMapId = -1;

                // Find the first ActionElementMap that maps to this Action and is compatible with this field type
                foreach(var actionElementMap in controllerMap.ElementMapsWithAction(action.id)) {
                    if(actionElementMap.ShowInField(row.actionRange)) {
                        name = actionElementMap.elementIdentifierName;
                        actionElementMapId = actionElementMap.id;
                        break;
                    }
                }

                // Set the label in the field button
                row.text.text = name;

                // Set the field button callback
                row.button.onClick.RemoveAllListeners(); // clear the button event listeners first
                int index = i; // copy variable for closure
                row.button.onClick.AddListener(() => OnInputFieldClicked(index, actionElementMapId));
            }
        }

        private void ClearUI() {

            // Clear the controller name
            if(selectedControllerType == ControllerType.Joystick) controllerNameUIText.text = "No joysticks attached";
            else controllerNameUIText.text = string.Empty;

            // Clear button labels
            for(int i = 0; i < rows.Count; i++) {
                rows[i].text.text = string.Empty;
            }
        }

        private void InitializeUI() {

            // Delete placeholders
            foreach(Transform t in actionGroupTransform) {
                Object.Destroy(t.gameObject);
            }
            foreach(Transform t in fieldGroupTransform) {
                Object.Destroy(t.gameObject);
            }

            // Create Action fields and input field buttons
            foreach(var action in ReInput.mapping.Actions) {
                if(action.type == InputActionType.Axis) {
                    // Create a full range, one positive, and one negative field for Axis-type Actions
                    CreateUIRow(action, AxisRange.Full, action.descriptiveName);
                    CreateUIRow(action, AxisRange.Positive, !string.IsNullOrEmpty(action.positiveDescriptiveName) ? action.positiveDescriptiveName : action.descriptiveName + " +");
                    CreateUIRow(action, AxisRange.Negative, !string.IsNullOrEmpty(action.negativeDescriptiveName) ? action.negativeDescriptiveName : action.descriptiveName + " -");
                } else if(action.type == InputActionType.Button) {
                    // Just create one positive field for Button-type Actions
                    CreateUIRow(action, AxisRange.Positive, action.descriptiveName);
                }
            }

            RedrawUI();
        }

        private void CreateUIRow(InputAction action, AxisRange actionRange, string label) {
            // Create the Action label
            GameObject labelGo = Object.Instantiate<GameObject>(textPrefab);
            labelGo.transform.SetParent(actionGroupTransform);
            labelGo.transform.SetAsLastSibling();
            labelGo.GetComponent<Text>().text = label;

            // Create the input field button
            GameObject buttonGo = Object.Instantiate<GameObject>(buttonPrefab);
            buttonGo.transform.SetParent(fieldGroupTransform);
            buttonGo.transform.SetAsLastSibling();

            // Add the row to the rows list
            rows.Add(
                new Row() {
                    action = action,
                    actionRange = actionRange,
                    button = buttonGo.GetComponent<Button>(),
                    text = buttonGo.GetComponentInChildren<Text>()
                }
            );
        }

        private void SetSelectedController(ControllerType controllerType) {
            bool changed = false;

            // Check if the controller type changed
            if(controllerType != selectedControllerType) { // controller type changed
                selectedControllerType = controllerType;
                changed = true;
            }

            // Check if the controller id changed
            int origId = selectedControllerId;
            if(selectedControllerType == ControllerType.Joystick) {
                if(player.controllers.joystickCount > 0) selectedControllerId = player.controllers.Joysticks[0].id;
                else selectedControllerId = -1;
            } else {
                selectedControllerId = 0;
            }
            if(selectedControllerId != origId) changed = true;

            // If the controller changed, stop the input mapper and update the UI
            if(changed) {
                inputMapper.Stop();
                RedrawUI();
            }
        }

        // Event Handlers

        // Called by the controller UI Buttons when pressed
        public void OnControllerSelected(int controllerType) {
            SetSelectedController((ControllerType)controllerType);
        }

        // Called by the input field UI Button when pressed
        private void OnInputFieldClicked(int index, int actionElementMapToReplaceId) {
            if(index < 0 || index >= rows.Count) return; // index out of range
            if(controller == null) return; // there is no Controller selected

            // Begin listening for input
            inputMapper.Start(
                new InputMapper.Context() {
                    actionId = rows[index].action.id,
                    controllerMap = controllerMap,
                    actionRange = rows[index].actionRange,
                    actionElementMapToReplace = controllerMap.GetElementMap(actionElementMapToReplaceId)
                }
            );

            statusUIText.text = "Listening...";
        }

        private void OnControllerChanged(ControllerStatusChangedEventArgs args) {
            SetSelectedController(selectedControllerType);
        }

        private void OnInputMapped(InputMapper.InputMappedEventData data) {
            RedrawUI();
        }

        private void OnStopped(InputMapper.StoppedEventData data) {
            statusUIText.text = string.Empty;
        }

        // A small class to store information about the input field buttons
        private class Row {
            public InputAction action;
            public AxisRange actionRange;
            public Button button;
            public Text text;
        }
    }
}
// Copyright (c) 2018 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

/* This is a very basic example of control remapping using the InputMapper class
 * to combine keyboard and mouse bindings into a single column. This is not how
 * Rewired is intended to work, but because some developers want to present
 * it this way, this example has been provided.
 * 
 * This example only supports 1 Player, 1 mapping per Action, and only supports
 * keyboard and mouse input. This example is exclusively meant to illustrate how
 * to use multiple Input Mappers to poll for input on multiple input devices
 * simultaneously. It also illustrates how to handle replacement assignments
 * across controller types and Controller Maps.
 * 
 * No UI windows are used and conflict checking is handled automatically by
 * InputMapper to keep this example small and focused.
 */

namespace Rewired.Demos {

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;
    using System.Collections;

    [AddComponentMenu("")]
    public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour {

        private const string category = "Default";
        private const string layout = "Default";
        private const string uiCategory = "UI";

        private InputMapper inputMapper_keyboard = new InputMapper();
        private InputMapper inputMapper_mouse = new InputMapper();

        public GameObject buttonPrefab;
        public GameObject textPrefab;
        public RectTransform fieldGroupTransform;
        public RectTransform actionGroupTransform;
        public Text controllerNameUIText;
        public Text statusUIText;

        private List<Row> rows = new List<Row>();
        private TargetMapping _replaceTargetMapping;

        private Player player { get { return ReInput.players.GetPlayer(0); } }

        private void OnEnable() {
            if(!ReInput.isReady) return; // don't run if Rewired hasn't been initialized

            // Timeout after 5 seconds of listening
            inputMapper_keyboard.options.timeout = 5f;
            inputMapper_mouse.options.timeout = 5f;

            // Ignore Mouse X and Y axes
            inputMapper_mouse.options.ignoreMouseXAxis = true;
            inputMapper_mouse.options.ignoreMouseYAxis = true;

            // Prevent assigning buttons in full-axis fields
            inputMapper_keyboard.options.allowButtonsOnFullAxisAssignment = false;
            inputMapper_mouse.options.allowButtonsOnFullAxisAssignment = false;

            // Subscribe to events
            inputMapper_keyboard.InputMappedEvent += OnInputMapped;
            inputMapper_keyboard.StoppedEvent += OnStopped;
            inputMapper_mouse.InputMappedEvent += OnInputMapped;
            inputMapper_mouse.StoppedEvent += OnStopped;
            
            // Create UI elements
            InitializeUI();
        }

        private void OnDisable() {

            // Make sure the input mapper is stopped first
            inputMapper_keyboard.Stop();
            inputMapper_mouse.Stop();

            // Unsubscribe from events
            inputMapper_keyboard.RemoveAllEventListeners();
            inputMapper_mouse.RemoveAllEventListeners();
        }

        private void RedrawUI() {
            
            // Update joystick name in UI
            controllerNameUIText.text = "Keyboard/Mouse";

            // Update each button label with the currently mapped element identifier
            for(int i = 0; i < rows.Count; i++) {
                Row row = rows[i];
                InputAction action = rows[i].action;

                string name = string.Empty;
                int actionElementMapId = -1;

                // Find the first ActionElementMap that maps to this Action and is compatible with this field type
                for (int j = 0; j < 2; j++) {
                    // Search the Keyboard Map first, then the Mouse Map
                    ControllerType controllerType = j == 0 ? ControllerType.Keyboard : ControllerType.Mouse;
                    ControllerMap controllerMap = player.controllers.maps.GetMap(controllerType, 0, category, layout);
                    foreach (var actionElementMap in controllerMap.ElementMapsWithAction(action.id)) {
                        if (actionElementMap.ShowInField(row.actionRange)) {
                            name = actionElementMap.elementIdentifierName;
                            actionElementMapId = actionElementMap.id;
                            break;
                        }
                    }
                    if (actionElementMapId >= 0) break; // found one
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
            controllerNameUIText.text = string.Empty;

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
            foreach(var action in ReInput.mapping.ActionsInCategory(category)) {
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

        // Event Handlers

        // Called by the input field UI Button when pressed
        private void OnInputFieldClicked(int index, int actionElementMapToReplaceId) {
            if(index < 0 || index >= rows.Count) return; // index out of range

            ControllerMap keyboardMap = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, category, layout);
            ControllerMap mouseMap = player.controllers.maps.GetMap(ControllerType.Mouse, 0, category, layout);

            // Cannot replace a keyboard binding on a Mouse Map or vice versa
            // Replacement cross device has to be done by removing the other
            // binding manually after input is mapped.
            // Determine which map the replacement binding exists on and store that information.
            ControllerMap controllerMapWithReplacement;

            // Determine if the replacement is on the keyboard or mouse map
            if (keyboardMap.ContainsElementMap(actionElementMapToReplaceId)) controllerMapWithReplacement = keyboardMap;
            else if (mouseMap.ContainsElementMap(actionElementMapToReplaceId)) controllerMapWithReplacement = mouseMap;
            else controllerMapWithReplacement = null; // not a replacement

            // Store the information about the replacement if any
            _replaceTargetMapping = new TargetMapping() {
                actionElementMapId = actionElementMapToReplaceId,
                controllerMap = controllerMapWithReplacement
            };

            // Begin listening for input, but use a coroutine so it starts only after a short delay to prevent
            // the button bound to UI Submit from binding instantly when the input field is activated.
            StartCoroutine(StartListeningDelayed(index, keyboardMap, mouseMap, actionElementMapToReplaceId));
        }

        private IEnumerator StartListeningDelayed(int index, ControllerMap keyboardMap, ControllerMap mouseMap, int actionElementMapToReplaceId) {

            // Don't allow a binding for a short period of time after input field is activated
            // to prevent button bound to UI Submit from binding instantly when input field is activated.
            yield return new WaitForSeconds(0.1f);

            // Begin listening for input on both keyboard and mouse at the same time

            inputMapper_keyboard.Start(
                new InputMapper.Context() {
                    actionId = rows[index].action.id,
                    controllerMap = keyboardMap,
                    actionRange = rows[index].actionRange,
                    actionElementMapToReplace = keyboardMap.GetElementMap(actionElementMapToReplaceId)
                }
            );

            inputMapper_mouse.Start(
                new InputMapper.Context() {
                    actionId = rows[index].action.id,
                    controllerMap = mouseMap,
                    actionRange = rows[index].actionRange,
                    actionElementMapToReplace = mouseMap.GetElementMap(actionElementMapToReplaceId)
                }
            );

            // Disable the UI Controller Maps while listening to prevent UI control and submissions.
            player.controllers.maps.SetMapsEnabled(false, uiCategory);

            // Update the UI text
            statusUIText.text = "Listening...";
        }

        private void OnInputMapped(InputMapper.InputMappedEventData data) {

            // Stop both mappers so they're no longer listening
            inputMapper_keyboard.Stop();
            inputMapper_mouse.Stop();

            // Handle cross device type binding replacement
            if(_replaceTargetMapping.controllerMap != null) { // we have a replacement

                // Remove the replacement from the other map if the binding was made on the opposite device type
                if(data.actionElementMap.controllerMap != _replaceTargetMapping.controllerMap) {
                    _replaceTargetMapping.controllerMap.DeleteElementMap(_replaceTargetMapping.actionElementMapId);
                }
            }

            RedrawUI();
        }

        private void OnStopped(InputMapper.StoppedEventData data) {
            statusUIText.text = string.Empty;

            // Re-enable UI Controller Maps after listening is finished.
            player.controllers.maps.SetMapsEnabled(true, uiCategory);
        }

        // A small class to store information about the input field buttons
        private class Row {
            public InputAction action;
            public AxisRange actionRange;
            public Button button;
            public Text text;
        }

        private struct TargetMapping {
            public ControllerMap controllerMap;
            public int actionElementMapId;
        }
    }
}
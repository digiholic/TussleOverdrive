// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649 // disable warnings about unused variables

namespace Rewired.Demos {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    public class ControlRemappingDemo1 : MonoBehaviour {

        private const float defaultModalWidth = 250.0f;
        private const float defaultModalHeight = 200.0f;
        private const float assignmentTimeout = 5.0f;

        // Helper objects
        private DialogHelper dialog;

        // Listener
        private InputMapper inputMapper = new InputMapper();
        private InputMapper.ConflictFoundEventData conflictFoundEventData;

        // GUI state management
        private bool guiState;
        private bool busy;
        private bool pageGUIState;

        // Selections
        private Player selectedPlayer;
        private int selectedMapCategoryId;
        private ControllerSelection selectedController;
        ControllerMap selectedMap;

        // Other flags
        private bool showMenu;
        private bool startListening;

        // Scroll view positions
        private Vector2 actionScrollPos;
        private Vector2 calibrateScrollPos;

        // Queues
        private Queue<QueueEntry> actionQueue;

        // Setup vars
        private bool setupFinished;

        // Editor state management
        [System.NonSerialized]
        private bool initialized;
        private bool isCompiling;

        // Styles
        GUIStyle style_wordWrap;
        GUIStyle style_centeredBox;

        #region Initialization

        private void Awake() {
            inputMapper.options.timeout = assignmentTimeout;
            inputMapper.options.ignoreMouseXAxis = true;
            inputMapper.options.ignoreMouseYAxis = true;
            Initialize();
        }

        private void OnEnable() {
            Subscribe();
        }

        private void OnDisable() {
            Unsubscribe();
        }

        private void Initialize() {
            dialog = new DialogHelper();
            actionQueue = new Queue<QueueEntry>();
            selectedController = new ControllerSelection();
            ReInput.ControllerConnectedEvent += JoystickConnected;
            ReInput.ControllerPreDisconnectEvent += JoystickPreDisconnect; // runs before joystick is completely disconnected so we can save maps
            ReInput.ControllerDisconnectedEvent += JoystickDisconnected; // final disconnect that runs after joystick has been fully removed
            ResetAll();
            initialized = true;
            ReInput.userDataStore.Load(); // load saved user maps on start if there are any to load

            if(ReInput.unityJoystickIdentificationRequired) {
                IdentifyAllJoysticks();
            }
        }

        private void Setup() {
            if(setupFinished) return;

            // Create styles
            style_wordWrap = new GUIStyle(GUI.skin.label);
            style_wordWrap.wordWrap = true;
            style_centeredBox = new GUIStyle(GUI.skin.box);
            style_centeredBox.alignment = TextAnchor.MiddleCenter;

            setupFinished = true;
        }

        private void Subscribe() {
            Unsubscribe();
            inputMapper.ConflictFoundEvent += OnConflictFound;
            inputMapper.StoppedEvent += OnStopped;
        }

        private void Unsubscribe() {
            inputMapper.RemoveAllEventListeners();
        }

        #endregion

        #region Main Update

        public void OnGUI() {
#if UNITY_EDITOR
            // Check for script recompile in the editor
            CheckRecompile();
#endif

            if(!initialized) return;

            Setup();

            HandleMenuControl();

            if(!showMenu) {
                DrawInitialScreen();
                return;
            }

            SetGUIStateStart();

            // Process queue
            ProcessQueue();

            // Draw contents
            DrawPage();
            ShowDialog();

            SetGUIStateEnd();

            // Clear momentary vars
            busy = false;
        }

        #endregion

        #region Menu Control

        private void HandleMenuControl() {
            if(dialog.enabled) return; // don't allow closing the menu while dialog is open so there won't be issues remapping the Menu button

            if(Event.current.type == EventType.Layout) {
                if(ReInput.players.GetSystemPlayer().GetButtonDown("Menu")) {
                    if(showMenu) { // menu is open and will be closed
                        ReInput.userDataStore.Save(); // save all maps when menu is closed
                        Close();
                    } else {
                        Open();
                    }
                }
            }
        }

        private void Close() {

            ClearWorkingVars();
            showMenu = false;
        }

        private void Open() {
            showMenu = true;
        }

        #endregion

        #region Draw

        private void DrawInitialScreen() {
            GUIContent content;
            ActionElementMap map = ReInput.players.GetSystemPlayer().controllers.maps.GetFirstElementMapWithAction("Menu", true);

            if(map != null) {
                content = new GUIContent("Press " + map.elementIdentifierName + " to open the menu.");
            } else {
                content = new GUIContent("There is no element assigned to open the menu!");
            }

            // Draw the box
            GUILayout.BeginArea(GetScreenCenteredRect(300, 50));
            GUILayout.Box(content, style_centeredBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            GUILayout.EndArea();
        }

        private void DrawPage() {
            if(GUI.enabled != pageGUIState) GUI.enabled = pageGUIState;

            Rect screenRect = new Rect((Screen.width - (Screen.width * 0.9f)) * 0.5f, (Screen.height - (Screen.height * 0.9f)) * 0.5f, Screen.width * 0.9f, Screen.height * 0.9f);
            GUILayout.BeginArea(screenRect);

            // Player Selector
            DrawPlayerSelector();

            // Joystick Selector
            DrawJoystickSelector();

            // Mouse
            DrawMouseAssignment();

            // Controllers
            DrawControllerSelector();

            // Controller Calibration
            DrawCalibrateButton();

            // Categories
            DrawMapCategories();

            // Create scroll view
            actionScrollPos = GUILayout.BeginScrollView(actionScrollPos);

            // Actions
            DrawCategoryActions();

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void DrawPlayerSelector() {
            if(ReInput.players.allPlayerCount == 0) {
                GUILayout.Label("There are no players.");
                return;
            }

            GUILayout.Space(15);
            GUILayout.Label("Players:");
            GUILayout.BeginHorizontal();

            foreach(Player player in ReInput.players.GetPlayers(true)) {
                if(selectedPlayer == null) selectedPlayer = player;  // if no player selected, select first

                bool prevValue = player == selectedPlayer ? true : false;
                bool value = GUILayout.Toggle(prevValue, player.descriptiveName != string.Empty ? player.descriptiveName : player.name, "Button", GUILayout.ExpandWidth(false));
                if(value != prevValue) { // value changed
                    if(value) { // selected
                        selectedPlayer = player;
                        selectedController.Clear(); // reset the device selection
                        selectedMapCategoryId = -1; // clear category selection
                    } // do not allow deselection
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawMouseAssignment() {
            bool origGuiEnabled = GUI.enabled; // save GUI state
            if(selectedPlayer == null) GUI.enabled = false;

            GUILayout.Space(15);
            GUILayout.Label("Assign Mouse:");
            GUILayout.BeginHorizontal();

            bool prevValue = selectedPlayer != null && selectedPlayer.controllers.hasMouse ? true : false;
            bool value = GUILayout.Toggle(prevValue, "Assign Mouse", "Button", GUILayout.ExpandWidth(false));
            if(value != prevValue) { // user clicked
                if(value) {
                    selectedPlayer.controllers.hasMouse = true;
                    foreach(Player player in ReInput.players.Players) { // de-assign mouse from all players except System
                        if(player == selectedPlayer) continue; // skip self
                        player.controllers.hasMouse = false;
                    }
                } else {
                    selectedPlayer.controllers.hasMouse = false;
                }
            }

            GUILayout.EndHorizontal();
            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        private void DrawJoystickSelector() {
            bool origGuiEnabled = GUI.enabled; // save GUI state
            if(selectedPlayer == null) GUI.enabled = false;

            GUILayout.Space(15);
            GUILayout.Label("Assign Joysticks:");
            GUILayout.BeginHorizontal();

            bool prevValue = selectedPlayer == null || selectedPlayer.controllers.joystickCount == 0 ? true : false;
            bool value = GUILayout.Toggle(prevValue, "None", "Button", GUILayout.ExpandWidth(false));
            if(value != prevValue) { // user clicked
                selectedPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
                ControllerSelectionChanged();
                // do not allow deselection
            }

            if(selectedPlayer != null) {
                foreach(Joystick joystick in ReInput.controllers.Joysticks) {
                    prevValue = selectedPlayer.controllers.ContainsController(joystick);
                    value = GUILayout.Toggle(prevValue, joystick.name, "Button", GUILayout.ExpandWidth(false));
                    if(value != prevValue) { // user clicked
                        EnqueueAction(new JoystickAssignmentChange(selectedPlayer.id, joystick.id, value));
                    }
                }
            }

            GUILayout.EndHorizontal();
            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        private void DrawControllerSelector() {
            if(selectedPlayer == null) return;
            bool origGuiEnabled = GUI.enabled; // save GUI state

            GUILayout.Space(15);
            GUILayout.Label("Controller to Map:");
            GUILayout.BeginHorizontal();

            bool value, prevValue;

            if(!selectedController.hasSelection) {
                selectedController.Set(0, ControllerType.Keyboard); // select keyboard if nothing selected
                ControllerSelectionChanged();
            }

            // Keyboard
            prevValue = selectedController.type == ControllerType.Keyboard;
            value = GUILayout.Toggle(prevValue, "Keyboard", "Button", GUILayout.ExpandWidth(false));
            if(value != prevValue) {
                selectedController.Set(0, ControllerType.Keyboard); // set current selected device to this
                ControllerSelectionChanged();
            }

            // Mouse
            if(!selectedPlayer.controllers.hasMouse) GUI.enabled = false; // disable mouse if player doesn't have mouse assigned
            prevValue = selectedController.type == ControllerType.Mouse;
            value = GUILayout.Toggle(prevValue, "Mouse", "Button", GUILayout.ExpandWidth(false));
            if(value != prevValue) {
                selectedController.Set(0, ControllerType.Mouse); // set current selected device to this
                ControllerSelectionChanged();
            }
            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // re-enable gui

            // Joystick List
            foreach(Joystick j in selectedPlayer.controllers.Joysticks) {
                prevValue = selectedController.type == ControllerType.Joystick && selectedController.id == j.id;
                value = GUILayout.Toggle(prevValue, j.name, "Button", GUILayout.ExpandWidth(false));
                if(value != prevValue) {
                    selectedController.Set(j.id, ControllerType.Joystick); // set current selected device to this
                    ControllerSelectionChanged();
                }
            }

            GUILayout.EndHorizontal();
            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        private void DrawCalibrateButton() {
            if(selectedPlayer == null) return;
            bool origGuiEnabled = GUI.enabled; // save GUI state

            GUILayout.Space(10);

            Controller controller = selectedController.hasSelection ? selectedPlayer.controllers.GetController(selectedController.type, selectedController.id) : null;

            if(controller == null || selectedController.type != ControllerType.Joystick) {
                GUI.enabled = false;
                GUILayout.Button("Select a controller to calibrate", GUILayout.ExpandWidth(false));
                if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled;
            } else { // Calibrate joystick
                if(GUILayout.Button("Calibrate " + controller.name, GUILayout.ExpandWidth(false))) {
                    Joystick joystick = controller as Joystick;
                    if(joystick != null) {
                        CalibrationMap calibrationMap = joystick.calibrationMap;
                        if(calibrationMap != null) {
                            EnqueueAction(new Calibration(selectedPlayer, joystick, calibrationMap));
                        }
                    }
                }
            }

            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        private void DrawMapCategories() {
            if(selectedPlayer == null) return;
            if(!selectedController.hasSelection) return;
            bool origGuiEnabled = GUI.enabled; // save GUI state

            GUILayout.Space(15);
            GUILayout.Label("Categories:");
            GUILayout.BeginHorizontal();

            foreach(InputMapCategory category in ReInput.mapping.UserAssignableMapCategories) {
                if(!selectedPlayer.controllers.maps.ContainsMapInCategory(selectedController.type, category.id)) { // if player has no maps in this category for controller don't allow them to select it
                    GUI.enabled = false;
                } else {
                    // Select first available category if none selected
                    if(selectedMapCategoryId < 0) {
                        selectedMapCategoryId = category.id; // if no category selected, select first
                        selectedMap = selectedPlayer.controllers.maps.GetFirstMapInCategory(selectedController.type, selectedController.id, category.id);
                    }
                }

                bool prevValue = category.id == selectedMapCategoryId ? true : false;
                bool value = GUILayout.Toggle(prevValue, category.descriptiveName != string.Empty ? category.descriptiveName : category.name, "Button", GUILayout.ExpandWidth(false));
                if(value != prevValue) { // category changed
                    selectedMapCategoryId = category.id;
                    selectedMap = selectedPlayer.controllers.maps.GetFirstMapInCategory(selectedController.type, selectedController.id, category.id);
                }
                if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled;
            }

            GUILayout.EndHorizontal();
            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        private void DrawCategoryActions() {
            if(selectedPlayer == null) return;
            if(selectedMapCategoryId < 0) return;
            bool origGuiEnabled = GUI.enabled; // save GUI state
            if(selectedMap == null) return; // controller map does not exist for this category in this controller

            GUILayout.Space(15);
            GUILayout.Label("Actions:");

            InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(selectedMapCategoryId); // get the selected map category
            if(mapCategory == null) return;
            InputCategory actionCategory = ReInput.mapping.GetActionCategory(mapCategory.name); // get the action category with the same name
            if(actionCategory == null) return; // no action category exists with the same name

            float labelWidth = 150.0f;

            // Draw the list of actions for the selected action category
            foreach(InputAction action in ReInput.mapping.ActionsInCategory(actionCategory.id)) {
                string name = action.descriptiveName != string.Empty ? action.descriptiveName : action.name;

                if(action.type == InputActionType.Button) {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(name, GUILayout.Width(labelWidth));
                    DrawAddActionMapButton(selectedPlayer.id, action, AxisRange.Positive, selectedController, selectedMap); // Add assignment button

                    // Write out assigned elements
                    foreach(ActionElementMap elementMap in selectedMap.AllMaps) {
                        if(elementMap.actionId != action.id) continue;
                        DrawActionAssignmentButton(selectedPlayer.id, action, AxisRange.Positive, selectedController, selectedMap, elementMap);
                    }
                    GUILayout.EndHorizontal();

                } else if(action.type == InputActionType.Axis) { // Axis

                    // Draw main axis label and actions assigned to the full axis
                    if(selectedController.type != ControllerType.Keyboard) { // don't draw this for keyboards since keys can only be assigned to the +/- axes anyway

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(name, GUILayout.Width(labelWidth));
                        DrawAddActionMapButton(selectedPlayer.id, action, AxisRange.Full, selectedController, selectedMap); // Add assignment button

                        // Write out assigned elements
                        foreach(ActionElementMap elementMap in selectedMap.AllMaps) {
                            if(elementMap.actionId != action.id) continue;
                            if(elementMap.elementType == ControllerElementType.Button) continue; // skip buttons, will handle below
                            if(elementMap.axisType == AxisType.Split) continue; // skip split axes, will handle below
                            DrawActionAssignmentButton(selectedPlayer.id, action, AxisRange.Full, selectedController, selectedMap, elementMap);
                            DrawInvertButton(selectedPlayer.id, action, Pole.Positive, selectedController, selectedMap, elementMap);
                        }
                        GUILayout.EndHorizontal();
                    }

                    // Positive action
                    string positiveName = action.positiveDescriptiveName != string.Empty ? action.positiveDescriptiveName : action.descriptiveName + " +";
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(positiveName, GUILayout.Width(labelWidth));
                    DrawAddActionMapButton(selectedPlayer.id, action, AxisRange.Positive, selectedController, selectedMap); // Add assignment button

                    // Write out assigned elements
                    foreach(ActionElementMap elementMap in selectedMap.AllMaps) {
                        if(elementMap.actionId != action.id) continue;
                        if(elementMap.axisContribution != Pole.Positive) continue; // axis contribution is incorrect, skip
                        if(elementMap.axisType == AxisType.Normal) continue; // normal axes handled above
                        DrawActionAssignmentButton(selectedPlayer.id, action, AxisRange.Positive, selectedController, selectedMap, elementMap);
                    }
                    GUILayout.EndHorizontal();

                    // Negative action
                    string negativeName = action.negativeDescriptiveName != string.Empty ? action.negativeDescriptiveName : action.descriptiveName + " -";
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(negativeName, GUILayout.Width(labelWidth));
                    DrawAddActionMapButton(selectedPlayer.id, action, AxisRange.Negative, selectedController, selectedMap); // Add assignment button

                    // Write out assigned elements
                    foreach(ActionElementMap elementMap in selectedMap.AllMaps) {
                        if(elementMap.actionId != action.id) continue;
                        if(elementMap.axisContribution != Pole.Negative) continue; // axis contribution is incorrect, skip
                        if(elementMap.axisType == AxisType.Normal) continue; // normal axes handled above
                        DrawActionAssignmentButton(selectedPlayer.id, action, AxisRange.Negative, selectedController, selectedMap, elementMap);
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled; // restore GUI state
        }

        #endregion

        #region Buttons

        private void DrawActionAssignmentButton(int playerId, InputAction action, AxisRange actionRange, ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap) {

            if(GUILayout.Button(elementMap.elementIdentifierName, GUILayout.ExpandWidth(false), GUILayout.MinWidth(30.0f))) {
                InputMapper.Context context = new InputMapper.Context() {
                    actionId = action.id,
                    actionRange = actionRange,
                    controllerMap = controllerMap,
                    actionElementMapToReplace = elementMap
                };
                EnqueueAction(new ElementAssignmentChange(ElementAssignmentChangeType.ReassignOrRemove, context));
                startListening = true;
            }
            GUILayout.Space(4);
        }

        private void DrawInvertButton(int playerId, InputAction action, Pole actionAxisContribution, ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap) {
            bool value = elementMap.invert;
            bool newValue = GUILayout.Toggle(value, "Invert", GUILayout.ExpandWidth(false));
            if(newValue != value) {
                elementMap.invert = newValue;
            }
            GUILayout.Space(10);
        }

        private void DrawAddActionMapButton(int playerId, InputAction action, AxisRange actionRange, ControllerSelection controller, ControllerMap controllerMap) {
            if(GUILayout.Button("Add...", GUILayout.ExpandWidth(false))) {
                InputMapper.Context context = new InputMapper.Context() {
                    actionId = action.id,
                    actionRange = actionRange,
                    controllerMap = controllerMap
                };
                EnqueueAction(new ElementAssignmentChange(ElementAssignmentChangeType.Add, context));
                startListening = true;
            }
            GUILayout.Space(10);
        }

        #endregion

        #region Dialog Boxes

        private void ShowDialog() {
            dialog.Update();
        }

        #region Draw Window Functions

        private void DrawModalWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            // Buttons
            dialog.DrawConfirmButton("Okay");

            GUILayout.FlexibleSpace();
            dialog.DrawCancelButton();

            GUILayout.EndHorizontal();
        }

        private void DrawModalWindow_OkayOnly(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            // Buttons
            dialog.DrawConfirmButton("Okay");

            GUILayout.EndHorizontal();
        }

        private void DrawElementAssignmentWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            ElementAssignmentChange entry = actionQueue.Peek() as ElementAssignmentChange; // get item from queue
            if(entry == null) {
                dialog.Cancel();
                return;
            }

            float time;

            // Do not start until dialog is ready
            if(!dialog.busy) {

                // Start the listener
                if(startListening && inputMapper.status == InputMapper.Status.Idle) {
                    inputMapper.Start(entry.context);
                    startListening = false;
                }

                // Check for conflicts
                if(conflictFoundEventData != null) { // a conflict is pending
                    dialog.Confirm();
                    return;
                }

                time = inputMapper.timeRemaining;

                // Check for timeout
                if(time == 0f) {
                    dialog.Cancel();
                    return;
                }

            } else {
                time = inputMapper.options.timeout;
            }

            // Show the cancel timer
            GUILayout.Label("Assignment will be canceled in " + ((int)Mathf.Ceil(time)).ToString() + "...", style_wordWrap);
        }

        private void DrawElementAssignmentProtectedConflictWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            ElementAssignmentChange entry = actionQueue.Peek() as ElementAssignmentChange; // get item from queue
            if(entry == null) {
                dialog.Cancel();
                return;
            }

            // Draw Buttons
            GUILayout.BeginHorizontal();

            dialog.DrawConfirmButton(UserResponse.Custom1, "Add");
            GUILayout.FlexibleSpace();
            dialog.DrawCancelButton();

            GUILayout.EndHorizontal();
        }

        private void DrawElementAssignmentNormalConflictWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            ElementAssignmentChange entry = actionQueue.Peek() as ElementAssignmentChange; // get item from queue
            if(entry == null) {
                dialog.Cancel();
                return;
            }

            // Draw Buttons
            GUILayout.BeginHorizontal();

            dialog.DrawConfirmButton(UserResponse.Confirm, "Replace");
            GUILayout.FlexibleSpace();
            dialog.DrawConfirmButton(UserResponse.Custom1, "Add");
            GUILayout.FlexibleSpace();
            dialog.DrawCancelButton();

            GUILayout.EndHorizontal();
        }

        private void DrawReassignOrRemoveElementAssignmentWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            // Buttons
            dialog.DrawConfirmButton("Reassign");

            GUILayout.FlexibleSpace();

            dialog.DrawCancelButton("Remove");

            GUILayout.EndHorizontal();
        }

        private void DrawFallbackJoystickIdentificationWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            FallbackJoystickIdentification entry = actionQueue.Peek() as FallbackJoystickIdentification;
            if(entry == null) {
                dialog.Cancel();
                return;
            }

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.Label("Press any button or axis on \"" + entry.joystickName + "\" now.", style_wordWrap);

            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Skip")) {
                dialog.Cancel();
                return;
            }

            // Do not allow assignment until dialog is ready
            if(dialog.busy) return;

            // Remap the joystick input source
            bool success = ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(entry.joystickId, 0.8f, false);
            if(!success) return;

            // Finish
            dialog.Confirm();
        }

        private void DrawCalibrationWindow(string title, string message) {
            if(!dialog.enabled) return; // prevent this from running after dialog is closed

            Calibration entry = actionQueue.Peek() as Calibration;
            if(entry == null) {
                dialog.Cancel();
                return;
            }

            GUILayout.Space(5);

            // Message
            GUILayout.Label(message, style_wordWrap);

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            bool origGUIEnabled = GUI.enabled;

            // Controller element selection
            GUILayout.BeginVertical(GUILayout.Width(200));

            // Create a scroll view for the axis list in case using the default controller map which has a lot of axes
            calibrateScrollPos = GUILayout.BeginScrollView(calibrateScrollPos);

            if(entry.recording) GUI.enabled = false; // don't allow switching while recording min/max
            IList<ControllerElementIdentifier> axisIdentifiers = entry.joystick.AxisElementIdentifiers;
            for(int i = 0; i < axisIdentifiers.Count; i++) {
                ControllerElementIdentifier identifier = axisIdentifiers[i];
                bool isSelected = entry.selectedElementIdentifierId == identifier.id;
                bool newValue = GUILayout.Toggle(isSelected, identifier.name, "Button", GUILayout.ExpandWidth(false));
                if(isSelected != newValue) {
                    entry.selectedElementIdentifierId = identifier.id; // store the selection index
                }
            }
            if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled; // restore gui

            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            // Selected object information and controls
            GUILayout.BeginVertical(GUILayout.Width(200));

            if(entry.selectedElementIdentifierId >= 0) {

                float axisValue = entry.joystick.GetAxisRawById(entry.selectedElementIdentifierId);

                GUILayout.Label("Raw Value: " + axisValue.ToString());

                // Get the axis index from the element identifier id
                int axisIndex = entry.joystick.GetAxisIndexById(entry.selectedElementIdentifierId);
                AxisCalibration axis = entry.calibrationMap.GetAxis(axisIndex); // get the axis calibration

                // Show current axis information
                GUILayout.Label("Calibrated Value: " + entry.joystick.GetAxisById(entry.selectedElementIdentifierId));
                GUILayout.Label("Zero: " + axis.calibratedZero);
                GUILayout.Label("Min: " + axis.calibratedMin);
                GUILayout.Label("Max: " + axis.calibratedMax);
                GUILayout.Label("Dead Zone: " + axis.deadZone);

                GUILayout.Space(15);

                // Enabled -- allows user to disable an axis entirely if its giving them problems
                bool newEnabled = GUILayout.Toggle(axis.enabled, "Enabled", "Button", GUILayout.ExpandWidth(false));
                if(axis.enabled != newEnabled) {
                    axis.enabled = newEnabled;
                }

                GUILayout.Space(10);

                // Records Min/Max
                bool newRecording = GUILayout.Toggle(entry.recording, "Record Min/Max", "Button", GUILayout.ExpandWidth(false));
                if(newRecording != entry.recording) {
                    if(newRecording) { // just started recording
                        // Clear previous calibration so we can record min max from this session only
                        axis.calibratedMax = 0.0f;
                        axis.calibratedMin = 0.0f;
                    }
                    entry.recording = newRecording;
                }

                if(entry.recording) {
                    axis.calibratedMin = Mathf.Min(axis.calibratedMin, axisValue, axis.calibratedMin);
                    axis.calibratedMax = Mathf.Max(axis.calibratedMax, axisValue, axis.calibratedMax);
                    GUI.enabled = false;
                }

                // Set Zero state
                if(GUILayout.Button("Set Zero", GUILayout.ExpandWidth(false))) {
                    axis.calibratedZero = axisValue;
                }

                // Set Dead Zone
                if(GUILayout.Button("Set Dead Zone", GUILayout.ExpandWidth(false))) {
                    axis.deadZone = axisValue;
                }

                // Invert
                bool newInvert = GUILayout.Toggle(axis.invert, "Invert", "Button", GUILayout.ExpandWidth(false));
                if(axis.invert != newInvert) {
                    axis.invert = newInvert;
                }

                GUILayout.Space(10);
                if(GUILayout.Button("Reset", GUILayout.ExpandWidth(false))) {
                    axis.Reset();
                }

                if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled;

            } else {
                GUILayout.Label("Select an axis to begin.");
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            // Close Button
            GUILayout.FlexibleSpace();
            if(entry.recording) GUI.enabled = false;
            if(GUILayout.Button("Close")) {
                calibrateScrollPos = new Vector2(); // clear the scroll view position
                dialog.Confirm();
            }
            if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled;
        }

        #endregion

        #region Result Callbacks

        private void DialogResultCallback(int queueActionId, UserResponse response) {
            foreach(QueueEntry entry in actionQueue) { // find the right one and cancel or confirm it
                if(entry.id != queueActionId) continue;
                if(response != UserResponse.Cancel) entry.Confirm(response); // mark the entry as confirmed and record user response
                else entry.Cancel(); // mark the entry as canceled
                break;
            }
        }

        #endregion

        #region Misc

        private Rect GetScreenCenteredRect(float width, float height) {
            return new Rect(
                (float)(Screen.width * 0.5f - width * 0.5f),
                (float)(Screen.height * 0.5 - height * 0.5f),
                width,
                height
            );
        }

        #endregion

        #endregion

        #region Action Queue

        private void EnqueueAction(QueueEntry entry) {
            if(entry == null) return;
            busy = true;
            GUI.enabled = false; // disable control on everything until next cycle
            actionQueue.Enqueue(entry);
        }

        private void ProcessQueue() {
            if(dialog.enabled) return; // dialog is open, do not process queue
            if(busy || actionQueue.Count == 0) return;

            while(actionQueue.Count > 0) {
                QueueEntry queueEntry = actionQueue.Peek(); // get next item from queue

                bool goNext = false;

                // Process different types of actions
                switch(queueEntry.queueActionType) {
                    case QueueActionType.JoystickAssignment:
                        goNext = ProcessJoystickAssignmentChange((JoystickAssignmentChange)queueEntry);
                        break;
                    case QueueActionType.ElementAssignment:
                        goNext = ProcessElementAssignmentChange((ElementAssignmentChange)queueEntry);
                        break;
                    case QueueActionType.FallbackJoystickIdentification:
                        goNext = ProcessFallbackJoystickIdentification((FallbackJoystickIdentification)queueEntry);
                        break;
                    case QueueActionType.Calibrate:
                        goNext = ProcessCalibration((Calibration)queueEntry);
                        break;
                }

                // Quit processing the queue if we opened a modal
                if(!goNext) break;

                // Remove item from queue since we're done with it
                actionQueue.Dequeue();
            }
        }

        private bool ProcessJoystickAssignmentChange(JoystickAssignmentChange entry) {

            // Handle user cancelation
            if(entry.state == QueueEntry.State.Canceled) { // action was canceled
                return true;
            }

            Player player = ReInput.players.GetPlayer(entry.playerId);
            if(player == null) return true;

            if(!entry.assign) { // deassign joystick
                player.controllers.RemoveController(ControllerType.Joystick, entry.joystickId);
                ControllerSelectionChanged();
                return true;
            }

            // Assign joystick
            if(player.controllers.ContainsController(ControllerType.Joystick, entry.joystickId)) return true; // same player, nothing to change
            bool alreadyAssigned = ReInput.controllers.IsJoystickAssigned(entry.joystickId);

            if(!alreadyAssigned || entry.state == QueueEntry.State.Confirmed) { // not assigned or user confirmed the action already, do it
                player.controllers.AddController(ControllerType.Joystick, entry.joystickId, true);
                ControllerSelectionChanged();
                return true;
            }

            // Create dialog and start waiting for user confirmation
            dialog.StartModal(entry.id, DialogHelper.DialogType.JoystickConflict, new WindowProperties {
                title = "Joystick Reassignment",
                message = "This joystick is already assigned to another player. Do you want to reassign this joystick to " + player.descriptiveName + "?",
                rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                windowDrawDelegate = DrawModalWindow
            },
            DialogResultCallback);
            return false; // don't process anything more in this queue
        }

        private bool ProcessElementAssignmentChange(ElementAssignmentChange entry) {

            switch(entry.changeType) {
                case ElementAssignmentChangeType.ReassignOrRemove:
                    return ProcessRemoveOrReassignElementAssignment(entry);
                case ElementAssignmentChangeType.Remove:
                    return ProcessRemoveElementAssignment(entry);
                case ElementAssignmentChangeType.Add:
                case ElementAssignmentChangeType.Replace:
                    return ProcessAddOrReplaceElementAssignment(entry);
                case ElementAssignmentChangeType.ConflictCheck:
                    return ProcessElementAssignmentConflictCheck(entry);
                default:
                    throw new System.NotImplementedException();
            }
        }

        private bool ProcessRemoveOrReassignElementAssignment(ElementAssignmentChange entry) {
            if(entry.context.controllerMap == null) return true;
            if(entry.state == QueueEntry.State.Canceled) { // delete entry
                // Enqueue a new action to delete the entry
                ElementAssignmentChange newEntry = new ElementAssignmentChange(entry); // copy the entry
                newEntry.changeType = ElementAssignmentChangeType.Remove; // change the type to Remove
                actionQueue.Enqueue(newEntry); // enqueue the new entry
                return true;
            }

            // Check for user confirmation
            if(entry.state == QueueEntry.State.Confirmed) { // reassign entry
                // Enqueue a new action to reassign the entry
                ElementAssignmentChange newEntry = new ElementAssignmentChange(entry); // copy the entry
                newEntry.changeType = ElementAssignmentChangeType.Replace; // change the type to Replace
                actionQueue.Enqueue(newEntry); // enqueue the new entry
                return true;
            }

            // Create dialog and start waiting for user assignment
            dialog.StartModal(entry.id, DialogHelper.DialogType.AssignElement, new WindowProperties {
                title = "Reassign or Remove",
                message = "Do you want to reassign or remove this assignment?",
                rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                windowDrawDelegate = DrawReassignOrRemoveElementAssignmentWindow
            },
            DialogResultCallback);

            return false;
        }

        private bool ProcessRemoveElementAssignment(ElementAssignmentChange entry) {
            if(entry.context.controllerMap == null) return true;
            if(entry.state == QueueEntry.State.Canceled) return true; // user canceled

            // Delete element
            if(entry.state == QueueEntry.State.Confirmed) { // user confirmed, delete it
                entry.context.controllerMap.DeleteElementMap(entry.context.actionElementMapToReplace.id);
                return true;
            }

            // Create dialog and start waiting for user confirmation
            dialog.StartModal(entry.id, DialogHelper.DialogType.DeleteAssignmentConfirmation, new WindowProperties {
                title = "Remove Assignment",
                message = "Are you sure you want to remove this assignment?",
                rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                windowDrawDelegate = DrawModalWindow
            },
            DialogResultCallback);
            return false; // don't process anything more in this queue
        }

        private bool ProcessAddOrReplaceElementAssignment(ElementAssignmentChange entry) {

            if(entry.state == QueueEntry.State.Canceled) {
                inputMapper.Stop();
                return true; // user canceled
            }

            // Check for user confirmation
            if(entry.state == QueueEntry.State.Confirmed) { // the action assignment has been confirmed
                if(Event.current.type != EventType.Layout) return false; // only make changes in layout to avoid GUI errors when new controls appear

                // Handle conflicts
                if(conflictFoundEventData != null) { // we had conflicts
                    // Enqueue a conflict check
                    ElementAssignmentChange newEntry = new ElementAssignmentChange(entry); // clone the entry
                    newEntry.changeType = ElementAssignmentChangeType.ConflictCheck; // set the new type to check for conflicts
                    actionQueue.Enqueue(newEntry); // enqueue the new entry
                }

                return true; // finished
            }

            // Customize the message for different controller types and different platforms
            string message;
            if(entry.context.controllerMap.controllerType == ControllerType.Keyboard) {

#if UNITY_5_4_OR_NEWER
                bool isOSX = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
#else
                bool isOSX = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer;
#endif
                if(isOSX) {
                    message = "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
                } else {
                    message = "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
                }

                // Editor modifier key disclaimer
                if(Application.isEditor) {
                    message += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
                }

            } else if(entry.context.controllerMap.controllerType == ControllerType.Mouse) {
                message = "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
            } else {
                message = "Press any button or axis to assign it to this action.";
            }

            // Create dialog and start waiting for user assignment
            dialog.StartModal(entry.id, DialogHelper.DialogType.AssignElement, new WindowProperties {
                title = "Assign",
                message = message,
                rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                windowDrawDelegate = DrawElementAssignmentWindow
            },
            DialogResultCallback);

            return false;
        }

        private bool ProcessElementAssignmentConflictCheck(ElementAssignmentChange entry) {
            if(entry.context.controllerMap == null) return true;
            if(entry.state == QueueEntry.State.Canceled) {
                inputMapper.Stop();
                return true; // user canceled
            }

            if(conflictFoundEventData == null) return true; // error

            // Check for user confirmation
            if(entry.state == QueueEntry.State.Confirmed) {

                if(entry.response == UserResponse.Confirm) { // remove and add
                    conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Replace);
                } else if(entry.response == UserResponse.Custom1) { // add without removing
                    conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Add);
                } else throw new System.NotImplementedException();

                return true; // finished
            }

            // Open a different dialog depending on if a protected conflict was found
            if(conflictFoundEventData.isProtected) {
                string message = conflictFoundEventData.assignment.elementDisplayName + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";

                // Create dialog and start waiting for user assignment
                dialog.StartModal(entry.id, DialogHelper.DialogType.AssignElement, new WindowProperties {
                    title = "Assignment Conflict",
                    message = message,
                    rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                    windowDrawDelegate = DrawElementAssignmentProtectedConflictWindow
                },
                DialogResultCallback);

            } else {
                string message = conflictFoundEventData.assignment.elementDisplayName + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";

                // Create dialog and start waiting for user assignment
                dialog.StartModal(entry.id, DialogHelper.DialogType.AssignElement, new WindowProperties {
                    title = "Assignment Conflict",
                    message = message,
                    rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                    windowDrawDelegate = DrawElementAssignmentNormalConflictWindow
                },
                DialogResultCallback);

            }

            return false;
        }

        private bool ProcessFallbackJoystickIdentification(FallbackJoystickIdentification entry) {
            // Handle user cancelation
            if(entry.state == QueueEntry.State.Canceled) { // action was canceled
                return true;
            }

            // Identify joystick
            if(entry.state == QueueEntry.State.Confirmed) {
                // nothing to do, done
                return true;
            }

            // Create dialog and start waiting for user confirmation
            dialog.StartModal(entry.id, DialogHelper.DialogType.JoystickConflict, new WindowProperties {
                title = "Joystick Identification Required",
                message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
                rect = GetScreenCenteredRect(defaultModalWidth, defaultModalHeight),
                windowDrawDelegate = DrawFallbackJoystickIdentificationWindow
            },
            DialogResultCallback,
            1.0f); // add a longer delay after the dialog opens to prevent one joystick press from being used for subsequent joysticks if held for a short time
            return false; // don't process anything more in this queue
        }

        private bool ProcessCalibration(Calibration entry) {
            // Handle user cancelation
            if(entry.state == QueueEntry.State.Canceled) { // action was canceled
                return true;
            }

            if(entry.state == QueueEntry.State.Confirmed) {
                return true;
            }

            // Create dialog and start waiting for user confirmation
            dialog.StartModal(entry.id, DialogHelper.DialogType.JoystickConflict, new WindowProperties {
                title = "Calibrate Controller",
                message = "Select an axis to calibrate on the " + entry.joystick.name + ".",
                rect = GetScreenCenteredRect(450, 480),
                windowDrawDelegate = DrawCalibrationWindow
            },
            DialogResultCallback);
            return false; // don't process anything more in this queue
        }

        #endregion

        #region Selection Chaging

        private void PlayerSelectionChanged() {
            ClearControllerSelection();
        }

        private void ControllerSelectionChanged() {
            ClearMapSelection();
        }

        private void ClearControllerSelection() {
            selectedController.Clear(); // reset the device selection because joystick list will have changed
            ClearMapSelection();
        }

        #endregion

        #region Clear

        private void ClearMapSelection() {
            selectedMapCategoryId = -1; // clear map cat selection
            selectedMap = null;
        }

        private void ResetAll() {
            ClearWorkingVars();
            initialized = false;
            showMenu = false;
        }

        private void ClearWorkingVars() {
            selectedPlayer = null;
            ClearMapSelection();
            selectedController.Clear();
            actionScrollPos = new Vector2();
            dialog.FullReset();
            actionQueue.Clear();
            busy = false;
            startListening = false;
            conflictFoundEventData = null;
            inputMapper.Stop();
        }

        #endregion

        #region GUI State

        private void SetGUIStateStart() {
            guiState = true;
            if(busy) guiState = false;
            pageGUIState = guiState && !busy && !dialog.enabled && !dialog.busy; // enable page gui only if not busy and not in dialog mode
            if(GUI.enabled != guiState) GUI.enabled = guiState;
        }

        private void SetGUIStateEnd() {
            // always enable GUI again before exiting
            guiState = true;
            if(!GUI.enabled) GUI.enabled = guiState;
        }

        #endregion

        #region Joystick Connection Callbacks

        private void JoystickConnected(ControllerStatusChangedEventArgs args) {
            // Reload maps if a joystick is connected
            if(ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId)) {
                // Load the maps for the player(s) that are assigned this joystick
                foreach(Player player in ReInput.players.AllPlayers) {
                    if(player.controllers.ContainsController(args.controllerType, args.controllerId)) {
                        ReInput.userDataStore.LoadControllerData(player.id, args.controllerType, args.controllerId);
                    }
                }
            } else {
                // Just load the general joystick save data
                ReInput.userDataStore.LoadControllerData(args.controllerType, args.controllerId);
            }


            // Always force reidentification of all joysticks when a joystick is added or removed when using Unity input on a platform that requires manual identification
            if(ReInput.unityJoystickIdentificationRequired) IdentifyAllJoysticks();
        }

        private void JoystickPreDisconnect(ControllerStatusChangedEventArgs args) {
            // Check if the current editing controller was just disconnected and deselect it
            if(selectedController.hasSelection && args.controllerType == selectedController.type && args.controllerId == selectedController.id) {
                ClearControllerSelection(); // joystick was disconnected
            }

            // Save the user maps before the joystick is disconnected if in the menu since user may have changed something
            if(showMenu) {
                if(ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId)) {
                    foreach(Player player in ReInput.players.AllPlayers) {
                        if(!player.controllers.ContainsController(args.controllerType, args.controllerId)) continue;
                        ReInput.userDataStore.SaveControllerData(player.id, args.controllerType, args.controllerId);
                    }
                } else {
                    ReInput.userDataStore.SaveControllerData(args.controllerType, args.controllerId);
                }
            }
        }

        private void JoystickDisconnected(ControllerStatusChangedEventArgs args) {
            // Close dialogs and clear queue if a joystick is disconnected
            if(showMenu) ClearWorkingVars();

            // Always force reidentification of all joysticks when a joystick is added or removed when using Unity input
            if(ReInput.unityJoystickIdentificationRequired) IdentifyAllJoysticks();
        }

        #endregion

        #region Mapping Listener Event Handlers

        private void OnConflictFound(InputMapper.ConflictFoundEventData data) {
            this.conflictFoundEventData = data;
        }

        private void OnStopped(InputMapper.StoppedEventData data) {
            this.conflictFoundEventData = null;
        }

        #endregion

        #region Fallback Methods

        public void IdentifyAllJoysticks() {
            // Check if there are any joysticks
            if(ReInput.controllers.joystickCount == 0) return; // no joysticks, nothing to do

            // Clear all vars first which will clear dialogs and queues
            ClearWorkingVars();

            // Open the menu if its closed
            Open();

            // Enqueue the joysticks up for identification
            foreach(Joystick joystick in ReInput.controllers.Joysticks) {
                actionQueue.Enqueue(new FallbackJoystickIdentification(joystick.id, joystick.name)); // enqueue each joystick for identification
            }
        }

        #endregion

        #region Editor Methods

        protected void CheckRecompile() {
#if UNITY_EDITOR
            // Destroy system if recompiling
            if(UnityEditor.EditorApplication.isCompiling) { // editor is recompiling
                if(!isCompiling) { // this is the first cycle of recompile
                    ResetAll();
                    isCompiling = true;
                }
                GUILayout.Window(0, GetScreenCenteredRect(300, 100), RecompileWindow, new GUIContent("Scripts are Compiling"));
                return;
            }

            // Check for end of compile
            if(isCompiling) { // compiling is done
                isCompiling = false;
                Initialize();
            }
#endif
        }

        private void RecompileWindow(int windowId) {
#if UNITY_EDITOR
            GUILayout.FlexibleSpace();
            GUILayout.Label("Please wait while script compilation finishes...");
            GUILayout.FlexibleSpace();
#endif
        }

        #endregion

        #region Classes

        private class ControllerSelection {
            private int _id;
            private int _idPrev;
            private ControllerType _type;
            private ControllerType _typePrev;

            public ControllerSelection() {
                Clear();
            }

            public int id {
                get {
                    return _id;
                }
                set {
                    _idPrev = _id;
                    _id = value;
                }
            }

            public ControllerType type {
                get {
                    return _type;
                }
                set {
                    _typePrev = _type;
                    _type = value;
                }
            }

            public int idPrev { get { return _idPrev; } }
            public ControllerType typePrev { get { return _typePrev; } }
            public bool hasSelection { get { return _id >= 0; } }

            public void Set(int id, ControllerType type) {
                this.id = id;
                this.type = type;
            }

            public void Clear() {
                _id = -1;
                _idPrev = -1;
                _type = ControllerType.Joystick;
                _typePrev = ControllerType.Joystick;
            }
        }

        private class DialogHelper {
            private const float openBusyDelay = 0.25f; // a small delay after opening the window that prevents assignment input for a short time after window opens
            private const float closeBusyDelay = 0.1f; // a small after closing the window that the GUI will still be busy to prevent button clickthrough

            private DialogType _type;
            private bool _enabled;
            private float _busyTime;
            private bool _busyTimerRunning;
            private float busyTimer { get { if(!_busyTimerRunning) return 0.0f; return _busyTime - Time.realtimeSinceStartup; } }

            public bool enabled {
                get {
                    return _enabled;
                }
                set {
                    if(value) {
                        if(_type == DialogType.None) return; // cannot enable, no type set
                        StateChanged(openBusyDelay);
                    } else {
                        _enabled = value;
                        _type = DialogType.None;
                        StateChanged(closeBusyDelay);
                    }

                }
            }
            public DialogType type {
                get {
                    if(!_enabled) return DialogType.None;
                    return _type;
                }
                set {
                    if(value == DialogType.None) {
                        _enabled = false;
                        StateChanged(closeBusyDelay);
                    } else {
                        _enabled = true;
                        StateChanged(openBusyDelay);
                    }
                    _type = value;
                }
            }
            public bool busy { get { return _busyTimerRunning; } }

            private Action<int> drawWindowDelegate;
            private GUI.WindowFunction drawWindowFunction;
            private WindowProperties windowProperties;

            private int currentActionId;
            private Action<int, UserResponse> resultCallback;

            public DialogHelper() {
                drawWindowDelegate = DrawWindow;
                drawWindowFunction = new GUI.WindowFunction(drawWindowDelegate);
            }

            public void StartModal(int queueActionId, DialogType type, WindowProperties windowProperties, Action<int, UserResponse> resultCallback) {
                StartModal(queueActionId, type, windowProperties, resultCallback, -1.0f);
            }
            public void StartModal(int queueActionId, DialogType type, WindowProperties windowProperties, Action<int, UserResponse> resultCallback, float openBusyDelay) {
                currentActionId = queueActionId;
                this.windowProperties = windowProperties;
                this.type = type;
                this.resultCallback = resultCallback;
                if(openBusyDelay >= 0.0f) StateChanged(openBusyDelay); // override with user defined open busy delay
            }

            public void Update() {
                Draw();
                UpdateTimers();
            }

            public void Draw() {
                if(!_enabled) return;
                bool origGuiEnabled = GUI.enabled;
                GUI.enabled = true;
                GUILayout.Window(windowProperties.windowId, windowProperties.rect, drawWindowFunction, windowProperties.title);
                GUI.FocusWindow(windowProperties.windowId);
                if(GUI.enabled != origGuiEnabled) GUI.enabled = origGuiEnabled;
            }

            public void DrawConfirmButton() {
                DrawConfirmButton("Confirm");
            }
            public void DrawConfirmButton(string title) {
                bool origGUIEnabled = GUI.enabled; // store original gui state
                if(busy) GUI.enabled = false; // disable GUI if dialog is busy to prevent click though
                if(GUILayout.Button(title)) {
                    Confirm(UserResponse.Confirm);
                }
                if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled; // restore GUI
            }

            public void DrawConfirmButton(UserResponse response) {
                DrawConfirmButton(response, "Confirm");
            }
            public void DrawConfirmButton(UserResponse response, string title) {
                bool origGUIEnabled = GUI.enabled; // store original gui state
                if(busy) GUI.enabled = false; // disable GUI if dialog is busy to prevent click though
                if(GUILayout.Button(title)) {
                    Confirm(response);
                }
                if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled; // restore GUI
            }

            public void DrawCancelButton() {
                DrawCancelButton("Cancel");
            }
            public void DrawCancelButton(string title) {
                bool origGUIEnabled = GUI.enabled; // store original gui state
                if(busy) GUI.enabled = false; // disable GUI if dialog is busy to prevent click though
                if(GUILayout.Button(title)) {
                    Cancel();
                }
                if(GUI.enabled != origGUIEnabled) GUI.enabled = origGUIEnabled; // restore GUI
            }

            public void Confirm() {
                Confirm(UserResponse.Confirm);
            }
            public void Confirm(UserResponse response) {
                resultCallback(currentActionId, response);
                Close();
            }

            public void Cancel() {
                resultCallback(currentActionId, UserResponse.Cancel);
                Close();
            }

            private void DrawWindow(int windowId) {
                windowProperties.windowDrawDelegate(windowProperties.title, windowProperties.message);
            }

            private void UpdateTimers() {
                if(_busyTimerRunning) {
                    if(busyTimer <= 0.0f) _busyTimerRunning = false;
                }
            }

            private void StartBusyTimer(float time) {
                _busyTime = time + Time.realtimeSinceStartup;
                _busyTimerRunning = true;
            }

            private void Close() {
                Reset();
                StateChanged(closeBusyDelay);
            }

            private void StateChanged(float delay) {
                StartBusyTimer(delay);
            }

            private void Reset() {
                _enabled = false;
                _type = DialogType.None;
                currentActionId = -1;
                resultCallback = null;
            }

            private void ResetTimers() {
                _busyTimerRunning = false;
            }

            public void FullReset() {
                Reset();
                ResetTimers();
            }

            // Enums

            public enum DialogType {
                None = 0,
                JoystickConflict = 1,
                ElementConflict = 2,
                KeyConflict = 3,
                DeleteAssignmentConfirmation = 10,
                AssignElement = 11
            }
        }

        private abstract class QueueEntry {
            public int id { get; protected set; }
            public QueueActionType queueActionType { get; protected set; }
            public State state { get; protected set; }
            public UserResponse response { get; protected set; }

            private static int uidCounter;
            protected static int nextId {
                get {
                    int id = uidCounter;
                    uidCounter += 1;
                    return id;
                }
            }

            public QueueEntry(QueueActionType queueActionType) {
                id = nextId;
                this.queueActionType = queueActionType;
            }

            public void Confirm(UserResponse response) {
                state = State.Confirmed;
                this.response = response;
            }

            public void Cancel() {
                state = State.Canceled;
            }

            public enum State {
                Waiting = 0,
                Confirmed = 1,
                Canceled = 2
            }
        }

        private class JoystickAssignmentChange : QueueEntry {
            public int playerId { get; private set; }
            public int joystickId { get; private set; }
            public bool assign { get; private set; }

            public JoystickAssignmentChange(
                int newPlayerId,
                int joystickId,
                bool assign
            )
                : base(QueueActionType.JoystickAssignment) {
                this.playerId = newPlayerId;
                this.joystickId = joystickId;
                this.assign = assign;
            }
        }

        private class ElementAssignmentChange : QueueEntry {

            public ElementAssignmentChangeType changeType { get; set; }
            public InputMapper.Context context { get; private set; }

            public ElementAssignmentChange(ElementAssignmentChangeType changeType, InputMapper.Context context)
                : base(QueueActionType.ElementAssignment) {
                this.changeType = changeType;
                this.context = context;
            }

            public ElementAssignmentChange(ElementAssignmentChange other)
                : this(other.changeType, other.context.Clone()) {
            }
        }

        private class FallbackJoystickIdentification : QueueEntry {
            public int joystickId { get; private set; }
            public string joystickName { get; private set; }

            public FallbackJoystickIdentification(
                int joystickId,
                string joystickName
            )
                : base(QueueActionType.FallbackJoystickIdentification) {
                this.joystickId = joystickId;
                this.joystickName = joystickName;
            }
        }

        private class Calibration : QueueEntry {
            public Player player { get; private set; }
            public ControllerType controllerType { get; private set; }
            public Joystick joystick { get; private set; }
            public CalibrationMap calibrationMap { get; private set; }

            public int selectedElementIdentifierId;
            public bool recording;

            public Calibration(
                Player player,
                Joystick joystick,
                CalibrationMap calibrationMap
            )
                : base(QueueActionType.Calibrate) {
                this.player = player;
                this.joystick = joystick;
                this.calibrationMap = calibrationMap;
                selectedElementIdentifierId = -1;
            }
        }

        private struct WindowProperties {
            public int windowId;
            public Rect rect;
            public Action<string, string> windowDrawDelegate;
            public string title;
            public string message;
        }

        #endregion

        #region Enums

        private enum QueueActionType {
            None = 0,
            JoystickAssignment = 1,
            ElementAssignment = 2,
            FallbackJoystickIdentification = 3,
            Calibrate = 4
        }

        private enum ElementAssignmentChangeType {
            Add = 0,
            Replace = 1,
            Remove = 2,
            ReassignOrRemove = 3,
            ConflictCheck = 4
        }


        public enum UserResponse {
            Confirm = 0,
            Cancel = 1,
            Custom1 = 2,
            Custom2 = 3
        }
        #endregion
    }
}
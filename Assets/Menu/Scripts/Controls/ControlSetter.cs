using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using TMPro;

public class ControlSetter : MonoBehaviour {
    public static ControlSetter current_setter;

    private string[] keyboard_layouts = new string[] { "p1_default", "p2_default", "p3_default", "p4_default" };
    private InputMapper inputMapper = new InputMapper();

    public Player player { get { return ReInput.players.GetPlayer(player_num); } }
    public Controller controller;

    public TextMeshProUGUI statusText;

    public ControllerMap controllerMap
    {
        get
        {
            if (controller == null) return null;
            if (controller.type == ControllerType.Keyboard)
                return player.controllers.maps.GetMap(controller, "Default", keyboard_layouts[player_num]);
            return player.controllers.maps.GetMap(controller, "Default", "Default");
        }
    }


    private int player_num = 0;
    private int controller_index = 0;
    private List<Controller> controllers = new List<Controller>();
    private int controller_count { get { return controllers.Count; } }

    private int temp_controller_index = 0;
    public Controller temp_controller;

    void Start()
    {
        current_setter = this;
        controller = ReInput.controllers.GetController(ControllerType.Keyboard, 0);
        temp_controller = ReInput.controllers.GetController(ControllerType.Keyboard, 0);
        LoadControllerList();
        UpdateChildren();
    }

    public void ChangePlayer(int value)
    {
        player_num += value;
        if (player_num < 0) player_num = 4 + player_num;
        player_num = player_num % 4;
        UpdateChildren();
    }

    public void ChangeController(int value)
    {
        controller_index += value;
        if (controller_index < 0) controller_index = controller_count + controller_index;
        if (controller_count > 0) controller_index = controller_index % controller_count;

        controller = controllers[controller_index];

        
        if (controller.type == ControllerType.Joystick)
        {
            player.controllers.ClearControllersOfType(ControllerType.Joystick);
            player.controllers.AddController(controller, true);
        }
        
        UpdateChildren();
    }

    public void ChangeTempController(int value)
    {
        temp_controller_index += value;
        if (temp_controller_index < 0) temp_controller_index = controller_count + temp_controller_index;
        if (temp_controller_index > 0) temp_controller_index = temp_controller_index % controller_count;

        temp_controller = controllers[temp_controller_index];

        UpdateChildren();
    }

    public void ConfirmChangeController()
    {
        controller_index = temp_controller_index;
        controller = controllers[temp_controller_index];

        if (controller.type == ControllerType.Joystick)
        {
            player.controllers.ClearControllersOfType(ControllerType.Joystick);
            player.controllers.AddController(controller, true);
        }

        UpdateChildren();
    }

    public void MapKey(InputAction action, AxisRange axisRange, int actionElementMapToReplaceId)
    {
        statusText.text = "Listening for input...";
        MenuButtonNavigator.accept_inputs = false;
        controllerMap.DeleteElementMap(actionElementMapToReplaceId);
        inputMapper.Start(
            new InputMapper.Context()
            {
                actionId = action.id,
                controllerMap = controllerMap,
                actionRange = axisRange,
                actionElementMapToReplace = controllerMap.GetElementMap(actionElementMapToReplaceId)
            }
        );
    }

    void UpdateChildren()
    {
        BroadcastMessage("UpdateText");
    }


    void LoadControllerList()
    {
        controllers = new List<Controller>();

        foreach (Controller cont in ReInput.controllers.GetControllers(ControllerType.Keyboard))
            controllers.Add(cont);

        if (ReInput.controllers.GetControllers(ControllerType.Joystick) != null)
        {
            foreach (Controller cont in ReInput.controllers.GetControllers(ControllerType.Joystick))
                controllers.Add(cont);
        }
    }

    void OnEnable()
    {
        if (!ReInput.isReady) return; // don't run if Rewired hasn't been initialized

        // Timeout after 5 seconds of listening
        inputMapper.options.timeout = 5f;

        // Ignore Mouse X and Y axes
        inputMapper.options.ignoreMouseXAxis = true;
        inputMapper.options.ignoreMouseYAxis = true;

        // Subscribe to events
        inputMapper.InputMappedEvent += OnInputMapped;
        inputMapper.StoppedEvent += OnStopped;

        ReInput.ControllerConnectedEvent += OnControllerChanged;
        ReInput.ControllerDisconnectedEvent += OnControllerChanged;
    }

    void OnDisable()
    {
        ReInput.ControllerConnectedEvent -= OnControllerChanged;
        ReInput.ControllerDisconnectedEvent -= OnControllerChanged;
    }

    void OnControllerChanged(ControllerStatusChangedEventArgs args)
    {
        controller_index = 0;
        LoadControllerList();
        ChangeController(0);
    }

    private void OnInputMapped(InputMapper.InputMappedEventData data)
    {
        UpdateChildren();
    }

    private void OnStopped(InputMapper.StoppedEventData data)
    {
        statusText.text = string.Empty;
        StartCoroutine(EnableInputsAfterTime());
        UpdateChildren();
    }

    IEnumerator EnableInputsAfterTime()
    {
        yield return new WaitForSeconds(0.2f);
        MenuButtonNavigator.accept_inputs = true;
    }
}

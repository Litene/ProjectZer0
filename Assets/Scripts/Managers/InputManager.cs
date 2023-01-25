using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public enum ButtonState : byte{None, Pressed, Hold}
    //Todo need inputs
    public enum ButtonMapping {Interact, todo0, todo1, todo2, todo3}

    public struct InputData {
        private Vector2 _moveInputVector;
        private Vector2 _lookInputVector;
        private float _camZoomInput;
        private byte _interactInput;
        private byte _todo0Input;
        private byte _todo1Input;
        private byte _todo2Input;
        private byte _todo3Input;

        public static bool usingMouse = false;
        public static float ControllerLookSensitivity = 1;
        public static float MouseLookSensitivity = 1;
        public static float MouseZoomSensitivity = -.25f;

        public Vector2 MoveInput {
            get => _moveInputVector;
            set => _moveInputVector = value;
        }
        public void SetLookInput(Vector2 value) => _lookInputVector = value;
        public Vector2 GetLookInput(bool rawValue = false) => rawValue
            ? _lookInputVector
            : _lookInputVector * (usingMouse ? MouseLookSensitivity : ControllerLookSensitivity);
        public void SetCameraZoomInput(float value) => _camZoomInput = value; 
        public float GetCameraZoomInput(bool rawValue = false) =>
            rawValue ? _camZoomInput : _camZoomInput * MouseZoomSensitivity;
        
        public ButtonState GetButtonInput(ButtonMapping mapping) {
            switch (mapping) {
                case ButtonMapping.Interact: return (ButtonState)_interactInput;
                case ButtonMapping.todo0: return (ButtonState)_todo0Input;
                case ButtonMapping.todo1: return (ButtonState)_todo1Input;
                case ButtonMapping.todo2: return (ButtonState)_todo2Input;
                case ButtonMapping.todo3: return (ButtonState)_todo3Input;
                default: return 0;
            }
        }
        public void SetButtonInput(ButtonMapping mapping, ButtonState state) {
            switch (mapping) {
                case ButtonMapping.Interact: _interactInput = (byte)state; break;
                case ButtonMapping.todo0: _todo0Input = (byte)state; break;
                case ButtonMapping.todo1: _todo1Input = (byte)state; break;
                case ButtonMapping.todo2: _todo2Input = (byte)state; break;
                case ButtonMapping.todo3: _todo3Input = (byte)state; break;
            }
        }   
    }
}

public class InputManager : Singleton<InputManager> {
    private Inputs _inputs;
    private InputData _inputData;
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    
    private InputDevice _lastDevice;
    private readonly InputDevice _mouse = new Mouse();

    private void Awake() {
        _inputs ??= new Inputs();

        _inputData = new InputData();
        
        var player = _inputs.Player;

        player.Move.performed += ctx => _inputData.MoveInput = ctx.ReadValue<Vector2>();
        player.Move.canceled += _ => _inputData.MoveInput = Vector2.zero;
        player.Look.performed += ctx => _inputData.SetLookInput(ctx.ReadValue<Vector2>());
        player.Look.canceled += _ => _inputData.SetLookInput(Vector2.zero);

        player.Interact.started += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.Pressed);
        player.Interact.performed += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.Hold);
        player.Interact.canceled += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.None);

        player.CameraZoom.performed += ctx => _inputData.SetCameraZoomInput(ctx.ReadValue<float>());
        player.CameraZoom.canceled += _ => _inputData.SetCameraZoomInput(0);
        
        player.KeyboardInput.started += _ => SetInputDevice(_mouse);
        player.ControllerInput.started += ctx => SetInputDevice(ctx.control.device);
    }
    private void OnEnable() => _inputs.Player.Enable();
    private void OnDisable() => _inputs.Player.Disable();

    public Vector2 GetMoveInput() => _inputData.MoveInput;
    public Vector2 GetLookInput(bool rawValue = false) => _inputData.GetLookInput(rawValue);
    public float GetCameraZoomInput(bool rawValue = false) => _inputData.GetCameraZoomInput(rawValue);
    public ButtonState GetButtonInput(ButtonMapping mapping) => _inputData.GetButtonInput(mapping);
    public bool GetButtonInput(ButtonMapping mapping, ButtonState state) => _inputData.GetButtonInput(mapping) == state;
    public InputData GetInputData() => _inputData;
    
    public static float MouseSensitivity => InputData.MouseLookSensitivity;
    public static float GamepadLookSensitivity => InputData.ControllerLookSensitivity;
    private void SetInputDevice(InputDevice device) {
        if(device == _lastDevice)
            return;
        
        _lastDevice = device;
        InputData.usingMouse = device == _mouse;
    }
    public void BeginRebind(ButtonMapping buttonMapping) {
        _rebindingOperation = _inputs.FindAction(buttonMapping.ToString()).PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(.1f)
            .OnComplete(_ => RebindCompleted());

        _rebindingOperation.Start();
    }
    private void RebindCompleted() {
        _rebindingOperation.Dispose();
    }
}
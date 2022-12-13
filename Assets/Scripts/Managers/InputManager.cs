using Input;
using UnityEngine;

namespace Input {
    public enum ButtonState : byte{None, Pressed, Hold}
    //Todo need inputs
    public enum ButtonMapping{Interact}
    
    public struct InputData{
        private Vector2 _moveInputVector;
        private Vector2 _lookInputVector;
        private byte _interactInput;

        public Vector2 MoveInput {
            get => _moveInputVector;
            set => _moveInputVector = value;
        } 
        public Vector2 LookInput {
            get => _lookInputVector;
            set => _lookInputVector = value;
        }
        public ButtonState GetButtonInput(ButtonMapping mapping) {
            switch (mapping) {
                case ButtonMapping.Interact: return (ButtonState)_interactInput;
                default: return 0;
            }
        }
        public void SetButtonInput(ButtonMapping mapping, ButtonState state) {
            switch (mapping) {
                case ButtonMapping.Interact: _interactInput = (byte)state; break;
            }
        }   
    }
}

public class InputManager : Singleton<InputManager> {
    private Inputs _inputs;
    private InputData _inputData;
    
    private void Awake() {
        _inputs = new Inputs();

        _inputData = new InputData();

        var player = _inputs.Player;

        player.Move.performed += ctx => _inputData.MoveInput = ctx.ReadValue<Vector2>();
        player.Move.canceled += _ => _inputData.MoveInput = Vector2.zero;
        player.Look.performed += ctx => _inputData.LookInput = ctx.ReadValue<Vector2>();
        player.Look.canceled += _ => _inputData.LookInput = Vector2.zero;

        player.Interact.started += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.Pressed);
        player.Interact.performed += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.Hold);
        player.Interact.canceled += _ => _inputData.SetButtonInput(ButtonMapping.Interact, ButtonState.None);
    }
    private void OnEnable() => _inputs.Player.Enable();
    private void OnDisable() => _inputs.Player.Disable();

    public Vector2 GetMoveInput() => _inputData.MoveInput;
    public Vector2 GetLookInput() => _inputData.LookInput;
    public ButtonState GetButtonInput(ButtonMapping mapping) => _inputData.GetButtonInput(mapping);
    public InputData GetInputData() => _inputData;
}

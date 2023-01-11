using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Linq;

public enum WalkState {
    Running = 10,
    Walking = 5
}


public enum WalkModifier {
    Snow = 3,
    Indoor = 10
}
// scrapped
public class PlayerController : MonoBehaviour {
    private Animator _animator;
    private Vector2 _moveVector;
    private const float WalkingSpeed = 0.5f;
    private const float RunningSpeed = 1;
    [Range(0.5f, 5)] public float MaxAcceleration;
    public Vector2 SignedMoveVector;
    public Vector2 Velocity;
    private WalkState _currentWalkState = WalkState.Walking;
    private Transform _camTf;

    private float GetWalkSpeed => (int)_currentWalkState * 0.1f;

    private float _drag;

    private int _xSpeedHash = Animator.StringToHash("XSpeed");
    private int _zSpeedHash = Animator.StringToHash("ZSpeed");

    private void Awake() {
        _camTf = Camera.main.transform;
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _moveVector = CalculateMovement(InputManager.Instance.GetMoveInput());

        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) _currentWalkState = WalkState.Running;
        else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift)) _currentWalkState = WalkState.Walking;

        _animator.SetFloat(_xSpeedHash, _moveVector.x);
        _animator.SetFloat(_zSpeedHash, _moveVector.y);
    }

    private Vector2 CalculateMovement(Vector2 moveVector) {
        // can possible be solved through the input manager/input actions thingy
        Vector2 signedMovement = new Vector2(
            moveVector.x == 0 ? 0 : Mathf.Sign(moveVector.x), moveVector.y == 0 ? 0 : Mathf.Sign(moveVector.y));

        AlignWithCamera(signedMovement);
        Vector2 desiredVelocity = signedMovement * GetWalkSpeed;
        float maxSpeedChange = MaxAcceleration * Time.deltaTime;
        Velocity.x = Mathf.MoveTowards(Velocity.x, desiredVelocity.x, maxSpeedChange);
        Velocity.y = Mathf.MoveTowards(Velocity.y, desiredVelocity.y, maxSpeedChange);
        return Velocity;
    }

    private void AlignWithCamera(Vector2 inputVector) { //todo: lerp this shit
        if (inputVector == Vector2.zero) return;
        var rotationLR = _camTf.localEulerAngles;
        transform.rotation = Quaternion.AngleAxis(rotationLR.y, Vector3.up);


    }


}
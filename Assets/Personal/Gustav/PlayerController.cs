using System;
using UnityEngine;

public enum WalkState {
    Running = 10,
    Walking = 5
}

public enum WalkModifier {
    Snow = 3,
    Indoor = 10
}

public class PlayerController : MonoBehaviour {
    private Animator _animator;
    private Vector2 _moveVector;
    private const float WalkingSpeed = 0.5f;
    private const float RunningSpeed = 1;
    [Range(0.5f, 5)] public float MaxAcceleration;
    public Vector2 SignedMoveVector;
    public Vector2 Velocity;
    private WalkState _currentWalkState;
    private Transform _camTf;

    private float _drag;

    private int _xSpeedHash = Animator.StringToHash("XSpeed");
    private int _zSpeedHash = Animator.StringToHash("ZSpeed");
    
    private void Awake() {
        _camTf = Camera.main.transform;
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _moveVector = CalculateMovement(InputManager.Instance.GetMoveInput());
        _animator.SetFloat(_xSpeedHash, _moveVector.x);
        _animator.SetFloat(_zSpeedHash, _moveVector.y);
    }

    private Vector2 CalculateMovement(Vector2 moveVector) {
        // can possible be solved through the input manager/input actions thingy
        Vector2 signedMovement = new Vector2(
            moveVector.x == 0 ? 0 : Mathf.Sign(moveVector.x), moveVector.y == 0 ? 0 : Mathf.Sign(moveVector.y));
        Vector2 desiredVelocity = signedMovement * WalkingSpeed;
        float maxSpeedChange = MaxAcceleration * Time.deltaTime;
        Velocity.x = Mathf.MoveTowards(Velocity.x, desiredVelocity.x, maxSpeedChange);
        Velocity.y = Mathf.MoveTowards(Velocity.y, desiredVelocity.y, maxSpeedChange);
        return Velocity;
    }

    private Vector2 AlignWithCamera(Vector2 inputVector) {
        return new Vector2();
    }
}
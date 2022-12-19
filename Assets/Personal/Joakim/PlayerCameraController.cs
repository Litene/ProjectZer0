using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    public Transform target;
    private Vector3 _focusPoint;
    private Vector3 _currentVelocity = Vector3.zero;
    private const float CameraLookThreshold = 0.001f;
    [SerializeField] private float _mouseSensitivity = 15f;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField, Range(0f, 1f)] private float _focusCentering = 0.5f;
    [SerializeField, Min(0f)] private float _focusRadius = 1f;
    [SerializeField] private Vector2 _orbitAngles = new Vector2(45f, 0);
    [SerializeField, Range(1f, 360f)] private float _rotationSpeed = 90f;
    private void Awake() {
        _focusPoint = target.position;
    }

    void FixedUpdate() {
        UpdateFocusPoint();
        ManualRotation();
 
        Quaternion lookRotation = Quaternion.Euler(_orbitAngles);
        Vector3 lookDirection = transform.forward;
        Vector3 lookPosition = _focusPoint - lookDirection * _distanceToPlayer;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void ManualRotation() {
        Vector2 input = new Vector2(-InputManager.Instance.GetLookInput().normalized.y, InputManager.Instance.GetLookInput().normalized.x);
        
        if (input.x < -CameraLookThreshold || input.x > CameraLookThreshold || input.y < -CameraLookThreshold ||
            input.y > CameraLookThreshold) {
            _orbitAngles += _rotationSpeed * Time.unscaledDeltaTime * input;
        }
    }

    private void UpdateFocusPoint() {
        Vector3 targetPoint = target.position;
        _focusPoint = targetPoint;
        if (_focusRadius > 0f) {
            float distance = Vector3.Distance(targetPoint, _focusPoint);
            if (distance > _focusRadius) {

              // _focusPoint = Vector3.SmoothDamp(targetPoint, _focusPoint, ref _currentVelocity, (_focusRadius / distance) * Time.);
                _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, _focusRadius / distance);
            }
        }
        else {
            _focusPoint = targetPoint;
        }
    }

    private void CameraLook() {
        // InputManager.Instance.GetLookInput();
        Vector3 focusPoint = target.position;
        
        // Debug.Log(InputManager.Instance.GetLookInput(true));
        // transform.LookAt(target);
        // var normalizedLookInput = InputManager.Instance.GetLookInput().normalized;
        // var adjustedmoveSpeed = normalizedLookInput * (Time.deltaTime * mouseSensitivity);
        // transform.localPosition += new Vector3(adjustedmoveSpeed.x, 0, 0);
    }
}

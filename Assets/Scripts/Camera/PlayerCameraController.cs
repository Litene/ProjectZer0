using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

//depricated
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
    [SerializeField, Range(-89f, 89f)] private float minVerticalAngle = -30f, maxVerticalAngle = 60f;
    [SerializeField] [Range(0f, 1f)] private float _cameraTPosValue;
    [SerializeField] [Range(2, 20)] private float XOffSet;
    private float _lerpFromPlayer = 1f;
    private float _lerpToPlayer = 6f;
    private Vector3 _leftCamPos;
    private Vector3 _rightCamPos;
    private Vector3 _currentPanPoint;
    private Vector3 _targetPanPoint;
    private IEnumerator _panRoutine;

    private void Awake() {
        _focusPoint = target.position;
        transform.localRotation = Quaternion.Euler(_orbitAngles);
    }

    private void OnValidate() {
        if (maxVerticalAngle < minVerticalAngle) {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    private void Update() {
        _cameraTPosValue = Player.Instance.GetWeight();
    }

    void LateUpdate() {
        var position = target.position;
        _leftCamPos = new Vector3(position.x - XOffSet, position.y, position.z);
        _rightCamPos = new Vector3(position.x + XOffSet, position.y, position.z);
        // if (InputManager.Instance.GetLookInput().magnitude < 0.01f) {
        // }
        // else {
        //     transform.position = Vector3.Lerp(transform.position, _rightCamPos, _cameraTPosValue);
        // }
        UpdateFocusPoint();
        Quaternion lookRotation;
        if (ManualRotation()) {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(_orbitAngles);
        }
        else lookRotation = transform.localRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = _focusPoint - lookDirection * _distanceToPlayer;
        transform.position = Vector3.MoveTowards(transform.position, lookPosition, Time.deltaTime * (_cameraTPosValue != 0.5f ? _lerpFromPlayer * 5 : _lerpToPlayer * 20));
        transform.rotation = lookRotation;
        // transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void ConstrainAngles() {
        _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, minVerticalAngle, maxVerticalAngle);
        if (_orbitAngles.y < 0f) _orbitAngles.y += 360f;
        else if (_orbitAngles.y >= 360f) _orbitAngles.y -= 360f;
    }

    private bool ManualRotation() {
        Vector2 input = new Vector2(-InputManager.Instance.GetLookInput().normalized.y,
            InputManager.Instance.GetLookInput().normalized.x);

        if (input.magnitude > CameraLookThreshold) {
            _orbitAngles += input * (_rotationSpeed * Time.unscaledDeltaTime * _mouseSensitivity);
            return true;
        }

        return false;
    }

    private void UpdateFocusPoint() {
        _targetPanPoint = InputManager.Instance.GetLookInput().magnitude < 0.01f
            ? Vector3.Lerp(_leftCamPos, _rightCamPos, _cameraTPosValue)
            : target.position;
        _focusPoint = _targetPanPoint;

        if (_currentPanPoint != _targetPanPoint) {
           // if (_panRoutine != null) StopCoroutine(_panRoutine);
           // _panRoutine = PanCamera(_cameraTPosValue != 0.5f ? _lerpFromPlayer : _lerpToPlayer, _targetPanPoint);
           // StartCoroutine(_panRoutine);
        }

        //transform.position = Vector3.MoveTowards(transform.position, targetPoint,ref _currentVelocity, 0.1f * Time.deltaTime, 1);

        // if (_focusRadius > 0f) {
        //     float distance = Vector3.Distance(_targetPanPoint, _focusPoint);
        //     if (distance > _focusRadius) {
        //         // _focusPoint = Vector3.SmoothDamp(targetPoint, _focusPoint, ref _currentVelocity, (_focusRadius / distance) * Time.);
        //         _focusPoint = Vector3.Lerp(_targetPanPoint, _focusPoint, _focusRadius / distance);
        //     }
        // }
        // else {
        //     _focusPoint = _targetPanPoint;
        // }
    }

    private IEnumerator PanCamera(float lerpTime, Vector3 targetPos) {
        WaitForEndOfFrame frameSkip = new WaitForEndOfFrame();
        Vector3 signedDirection = targetPos - transform.position;
        while (signedDirection.magnitude > 0.1f) {
            // transform.position = Vector3.MoveTowards(transform.position, targetPos, lerpTime * Time.deltaTime);
            yield return frameSkip;
        }

        //transform.position = targetPos;
        _targetPanPoint = _currentPanPoint;
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
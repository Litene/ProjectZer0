using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNew3rdCam : MonoBehaviour {
    //All objects in the list will be shown in camera view
    public List<GameObject> currentFocusPoints = new List<GameObject>();

    [SerializeField, Tooltip("A point which the camera interpolates towards.")]
    private Transform leftPoint, rightPoint, farPoint;

    [SerializeField, Tooltip("The point which the camera looks towards.")]
    private Transform lookTarget;

    [Header("Player Character")] [SerializeField, Tooltip("A reference to the player character.")]
    private GameObject player;

    private Transform _currentTargetPoint;

    private Rigidbody _playerRb;

    [Header("Camera Settings")] [Range(5f, 20f)]
    public float smoothingFactor = 10f;

    private float _smoothing = 10f;
    private float _targetSmoothing;
    private Vector3 _velRef;

    private void Awake() {
        _playerRb = player.GetComponent<Rigidbody>();
    }

    private void Start() {
        _currentTargetPoint = leftPoint;
    }

    private void Update() {
        switch (_playerRb.velocity.x) {
            //This should ideally be a number just under the players max speed
            case < -7:

                _currentTargetPoint = rightPoint;

                break;

            //This should ideally be a number just under the players max speed
            case > 7:

                _currentTargetPoint = leftPoint;

                break;
        }

        _smoothing = Mathf.Lerp(_smoothing, _targetSmoothing, smoothingFactor / 2 * Time.deltaTime);
    }

    private void LateUpdate() {
        switch (currentFocusPoints.Count) {
            case > 0:

                //Locked camera mode

                break;

            case 0:
                if (currentFocusPoints.Count != 0) {
                    transform.position = Vector3.SmoothDamp(
                        transform.position, _currentTargetPoint.position, ref _velRef,
                        _smoothing * smoothingFactor * Time.deltaTime);
                    transform.LookAt(lookTarget);

                }
                else {
                    transform.LookAt(lookTarget);
                }

                break;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            _targetSmoothing = 15f;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _targetSmoothing = 5f;
        }
    }
}
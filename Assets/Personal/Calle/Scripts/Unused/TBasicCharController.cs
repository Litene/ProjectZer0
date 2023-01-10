using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBasicCharController : MonoBehaviour
{
    [SerializeField, Tooltip("A reference to the third person camera.")] private Camera thirdPersonCam;
    
    [SerializeField, Range(5f, 15f), Tooltip("Movement speed in units/second.")] private float speed = 10.0f;
    
    private Rigidbody _rb;
    
    void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update() {
        BasicMovement();
        
        //CameraRelativeMovement();
    }

    void BasicMovement() {
        float horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
        float verticalInput = UnityEngine.Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        _rb.velocity = moveDirection * speed;
    }
    
    void CameraRelativeMovement() {
        float horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
        float verticalInput = UnityEngine.Input.GetAxis("Vertical");

        Vector3 forward = thirdPersonCam.transform.forward.normalized;
        Vector3 right = thirdPersonCam.transform.right.normalized;

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 verticalForward = verticalInput * forward;
        Vector3 horizontalRight = horizontalInput * right;

        Vector3 cameraMovementDirection = verticalForward + horizontalRight;
        
        _rb.AddForce(cameraMovementDirection * speed, ForceMode.Acceleration);

        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -speed, speed), 0, Mathf.Clamp(_rb.velocity.z, -speed, speed));
    }
}

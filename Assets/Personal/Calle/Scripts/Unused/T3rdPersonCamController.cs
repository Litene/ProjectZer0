using System;
using UnityEngine;

public class T3rdPersonCamController : MonoBehaviour
{
    //Camera Targets
    
    [SerializeField, Tooltip("The transform of the position the camera should follow")]
    private Transform target;
    [SerializeField, Tooltip("The transform of the location/object the camera should look at")]
    private Transform lookAtTarget;
    [SerializeField, Tooltip("The transform of the location/object that the camera rotates around")]
    private Transform pivot;
    
    //Camera Smoothing and Position
    
    [SerializeField, Range(1f, 10f), Tooltip("The speed with which the camera catches up with its target")]
    private float smoothing = 5f;
    [SerializeField, Range(-2.5f, 2.5f), Tooltip("The height of the camera from the target")]
    private float cameraHeight = 0f;

    private Vector3 _heightOffset;
    private Vector3 _currentVelocity;

    //Camera Rotation

    [SerializeField, Range(1f, 10f), Tooltip("The speed with which the camera rotates around the pivot point")]
    private float rotationSpeed = 5f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        _heightOffset = new Vector3(0, cameraHeight, 0);
        
        /*Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
       
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothing * Time.deltaTime);*/
    }

    private void LateUpdate()
    {
        float rotationX = UnityEngine.Input.GetAxis("Mouse X") * rotationSpeed;
        pivot.RotateAround(pivot.position, Vector3.up, rotationX);
        
        Vector3 targetCamPos = target.position + _heightOffset;
    
        // Smoothly interpolate between the camera's current position and the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetCamPos, ref _currentVelocity, smoothing * Time.deltaTime);
    
        // Make the camera look at the lookAtTarget game object (player)
        transform.LookAt(lookAtTarget);
    }
}

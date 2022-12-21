using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {
    public Transform target;
    public Transform player;

    private Camera _camera;

    public float focusSpeed = 1;

    private Vector3 orbitForward;
    private Vector3 targetAngles;
    private Vector3 orbitOffset;
    private Vector3 panningOffset;

    private float zoomLevel = 10;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private float idleTimer = 0;

    private void Update()
    {
        OrbitCamera();
        ComposeCamera();
        
        transform.position = player.position + orbitOffset + panningOffset;
        transform.eulerAngles = targetAngles;
    }

    void OrbitCamera()
    {
        Vector2 lookInput = InputManager.Instance.GetLookInput();

        if (lookInput.sqrMagnitude != 0)
        {
            if(lookInput.magnitude > .25f)
            idleTimer = 0;
            
            targetAngles.x += -lookInput.y * Time.deltaTime * 15f;
            targetAngles.y += lookInput.x * Time.deltaTime * 15f;
            targetAngles.z = 0;

            targetAngles.x = Mathf.Clamp(targetAngles.x, -45f, 45f);

            if (targetAngles.y >= 360f)
                targetAngles.y -= 360f;
            else if (targetAngles.y < 0f)
                targetAngles.y += 360f;

            zoomLevel = Mathf.MoveTowards(zoomLevel, 10, Time.deltaTime*10f);
        }

        idleTimer += Time.deltaTime;

        Vector3 lookDirection = Quaternion.Euler(targetAngles)*Vector3.forward;

        orbitForward = lookDirection;
        orbitOffset = -lookDirection * zoomLevel;
    }

    void ComposeCamera()
    {
        if (idleTimer < .5f)
        {
            panningOffset = Vector3.MoveTowards(panningOffset, Vector3.zero, Time.deltaTime * focusSpeed * 1.25f);
            return;
        }
        
        targetAngles.x = Mathf.MoveTowardsAngle(targetAngles.x, 0, Time.deltaTime*15f);
        
        Matrix4x4 screenMatrix = _camera.projectionMatrix*_camera.worldToCameraMatrix;

        Vector3 targetCamPos = screenMatrix.MultiplyPoint(target.position);
        Vector3 playerCamPos = screenMatrix.MultiplyPoint(player.position);

        Vector2 playerToTargetPos = (targetCamPos - playerCamPos);

        float panSide = playerToTargetPos.x < 0 ? -1 : 1;

        float zoomDir = (Mathf.Abs(playerToTargetPos.x)-2/3f)*.5f+(Mathf.Abs(playerToTargetPos.y)-2/3f)*.5f;

        zoomDir = Mathf.Clamp(zoomDir, -.2f, .2f);

        zoomLevel += zoomDir * Time.deltaTime;

        Vector3 focusTarget = (new Vector3(2 / 6f*panSide, 2 / 6f));
        focusTarget.z = 0;

        focusTarget = screenMatrix.inverse.MultiplyVector(focusTarget)*zoomLevel;

        panningOffset = Vector3.MoveTowards(panningOffset, focusTarget,Time.deltaTime * focusSpeed);
    }

    private void OnDrawGizmos()
    {
        if(_camera == null)
            return;
        
        Matrix4x4 screenMatrix = (_camera.projectionMatrix*_camera.worldToCameraMatrix).inverse;
        
        Gizmos.DrawLine(screenMatrix.MultiplyPoint(new Vector3(-1, 2/6f)), screenMatrix.MultiplyPoint(new Vector3(1, 2/6f)));
        Gizmos.DrawLine(screenMatrix.MultiplyPoint(new Vector3(-1, -2/6f)), screenMatrix.MultiplyPoint(new Vector3(1, -2/6f)));
        Gizmos.DrawLine(screenMatrix.MultiplyPoint(new Vector3(-2/6f, 1)), screenMatrix.MultiplyPoint(new Vector3(-2/6f, -1)));
        Gizmos.DrawLine(screenMatrix.MultiplyPoint(new Vector3(2/6f, 1)), screenMatrix.MultiplyPoint(new Vector3(2/6f, -1)));
    }
}

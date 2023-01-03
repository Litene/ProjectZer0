using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {
    public Transform target;
    public Transform player;

    private Camera _camera;

    public float focusSpeed = 1;
    public float idlePitch = 10f;
    public float defaultZoom = 5f;

    private Vector3 orbitForward;
    private Vector3 targetAngles;
    private Vector3 orbitOffset;
    private Vector3 panningOffset;
    private Vector2 screenPanning;

    private float zoomLevel = 10;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        zoomLevel = defaultZoom;
    }

    private float idleTimer = 0;

    private void Update()
    {
        OrbitCamera();
        ComposeCamera();

        idleTimer = Mathf.Clamp01(idleTimer);
        transform.position = player.position + orbitOffset + panningOffset;
        transform.eulerAngles = targetAngles;
    }

    void OrbitCamera()
    {
        Vector2 lookInput = InputManager.Instance.GetLookInput();

        if (lookInput.sqrMagnitude != 0)
        {
            idleTimer -= Time.deltaTime * .25f;
            
            targetAngles.x += -lookInput.y * Time.deltaTime * 15f;
            targetAngles.y += lookInput.x * Time.deltaTime * 15f;
            targetAngles.z = 0;

            targetAngles.x = Mathf.Clamp(targetAngles.x, -45f, 45f);

            if (targetAngles.y >= 360f)
                targetAngles.y -= 360f;
            else if (targetAngles.y < 0f)
                targetAngles.y += 360f;
            
            if(idleTimer < .25f)
                zoomLevel = Mathf.MoveTowards(zoomLevel, defaultZoom, Time.deltaTime*10f);
        }
        else
            idleTimer += Time.deltaTime;

        Vector3 lookDirection = Quaternion.Euler(targetAngles)*Vector3.forward;

        orbitForward = lookDirection;
        orbitOffset = -lookDirection * zoomLevel;
    }

    void ComposeCamera()
    {
        Matrix4x4 screenMatrix = _camera.projectionMatrix*_camera.worldToCameraMatrix;

        Vector3 focusTarget;
        
        if (idleTimer < .5f)
        {
            focusTarget = (new Vector3(0, 1 / 6f));
            screenPanning = Vector2.MoveTowards(screenPanning, focusTarget, Time.deltaTime * focusSpeed*1.25f);

            panningOffset = screenMatrix.inverse.MultiplyVector(screenPanning)*zoomLevel;
            return;
        }
        
        targetAngles.x = Mathf.MoveTowardsAngle(targetAngles.x, _camera.fieldOfView*1/6f, Time.deltaTime*15f);

        Vector3 targetCamPos = screenMatrix.MultiplyPoint(target.position);
        Vector3 playerCamPos = screenMatrix.MultiplyPoint(player.position);

        Vector2 playerToTargetPos = (targetCamPos - playerCamPos);

        float panSide = playerToTargetPos.x < 0 ? -1 : 1;

        float zoomDir = (Mathf.Abs(playerToTargetPos.x)-2/3f)*.5f+(Mathf.Abs(playerToTargetPos.y)-2/3f)*.5f;

        zoomDir = Mathf.Clamp(zoomDir, -.2f, .2f);

        zoomLevel += zoomDir * Time.deltaTime;

        focusTarget = (new Vector3(2 / 6f*panSide, 2 / 6f, 0));

        screenPanning = Vector2.MoveTowards(screenPanning, focusTarget, Time.deltaTime * focusSpeed);

        panningOffset = screenMatrix.inverse.MultiplyVector(screenPanning)*zoomLevel;
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

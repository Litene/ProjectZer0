using System;
using Input;
using UnityEngine;

public class CameraTest : MonoBehaviour {
    private const float GridStep = 1/3f;
    private const float maxPitch = 45f;
    private readonly Vector2 defaultPan = new(0, GridStep/2f);
    
    public Transform target;
    public Transform player;

    private Camera _camera;
    private InputManager _inputManager;

    public float focusSpeed = 1;
    public float defaultZoom = 5f;

    private float _zoomOffset;
    private float _orbitDistance = 10;
    
    private Vector3 _targetAngles;
    private Vector3 _orbitOffset;
    private Vector3 _panningOffset;
    private Vector2 _screenPanning;

    public Vector3 CameraForward(bool includePitch = false) => includePitch
        ? Quaternion.Euler(_targetAngles) * Vector3.forward
        : Quaternion.AngleAxis(_targetAngles.y, Vector3.up) * Vector3.forward;
    
    public Quaternion CameraForwardRot(bool includePitch = false) => includePitch
        ? Quaternion.Euler(_targetAngles)
        : Quaternion.AngleAxis(_targetAngles.y, Vector3.up);
    
    private void Awake() {
        _camera = GetComponent<Camera>();
        _orbitDistance = defaultZoom;
        _inputManager = InputManager.Instance;
    }

    private float idleTimer = 0;

    private void Update() {
        OrbitCamera();
        ComposeCamera();

        idleTimer = Mathf.Clamp01(idleTimer);
        transform.position = player.position + _orbitOffset + _panningOffset;
        transform.eulerAngles = _targetAngles;
    }

    void OrbitCamera() {
        Vector2 lookInput = _inputManager.GetLookInput();
        _zoomOffset += _inputManager.GetCameraZoomInput() * Time.deltaTime;

        if (lookInput.sqrMagnitude != 0)
        {
            idleTimer -= Time.deltaTime * .25f;
            
            _targetAngles.x += -lookInput.y * Time.deltaTime * 15f;
            _targetAngles.y += lookInput.x * Time.deltaTime * 15f;
            _targetAngles.z = 0;

            _targetAngles.x = Mathf.Clamp(_targetAngles.x, -maxPitch, maxPitch);

            if (_targetAngles.y >= 360f)
                _targetAngles.y -= 360f;
            else if (_targetAngles.y < 0f)
                _targetAngles.y += 360f;
            
            if(idleTimer < .4f)
                _orbitDistance = Mathf.MoveTowards(_orbitDistance, defaultZoom, Time.deltaTime*10f);
        }
        else 
            idleTimer += idleTimer > .5f ? Time.deltaTime : Time.deltaTime*.5f;
        
        _zoomOffset += Mathf.Min(20f- _orbitDistance - _zoomOffset, 0);
        _zoomOffset += Mathf.Max(2f - _orbitDistance - _zoomOffset, 0);

        _orbitDistance = Mathf.Clamp(_orbitDistance, 2f, 20f);

        Vector3 lookDirection = Quaternion.Euler(_targetAngles)*Vector3.forward;
        
        _orbitOffset = -lookDirection * (_orbitDistance+_zoomOffset);
    }

    void ComposeCamera() {
        Matrix4x4 screenMatrix = _camera.projectionMatrix*_camera.worldToCameraMatrix;

        if (idleTimer < .5f)
        {
            _screenPanning = Vector2.MoveTowards(_screenPanning, defaultPan, Time.deltaTime * focusSpeed*1.25f);

            _panningOffset = screenMatrix.inverse.MultiplyVector(_screenPanning)*_orbitDistance;
            return;
        }

        _targetAngles.x = Mathf.MoveTowardsAngle(_targetAngles.x, _camera.fieldOfView*defaultPan.y, Time.deltaTime*15f);

        Vector3 targetCamPos = screenMatrix.MultiplyPoint(target.position);
        Vector3 playerCamPos = screenMatrix.MultiplyPoint(player.position);

        Vector2 playerToTargetPos = (targetCamPos - playerCamPos);

        float panSide = playerToTargetPos.x < 0 ? -1 : 1;

        float zoomDir = Mathf.Abs(playerToTargetPos.x)*.5f-GridStep+Mathf.Abs(playerToTargetPos.y)*.5f-GridStep;

        zoomDir = Mathf.Clamp(zoomDir, -.2f, .2f);

        _orbitDistance += zoomDir * Time.deltaTime*10f;

        Vector3 focusTarget = new Vector3(GridStep*panSide, GridStep);

        float distanceVelocity = Mathf.Pow(Vector2.Distance(_screenPanning, focusTarget)+.1f, 2);

        _screenPanning = Vector2.MoveTowards(_screenPanning, focusTarget, Time.deltaTime * focusSpeed*distanceVelocity);

        _panningOffset = screenMatrix.inverse.MultiplyVector(_screenPanning)*(_orbitDistance+_zoomOffset);
    }

    private void OnDrawGizmos() {
        if(_camera == null)
            return;
        
        Matrix4x4 screenMatrix = (_camera.projectionMatrix*_camera.worldToCameraMatrix).inverse;

        Vector3 upLeft = screenMatrix.MultiplyPoint(new Vector3(-1, GridStep));
        Vector3 upRight = screenMatrix.MultiplyPoint(new Vector3(1, GridStep));
        Vector3 downLeft = screenMatrix.MultiplyPoint(new Vector3(-1, -GridStep));
        Vector3 downRight = screenMatrix.MultiplyPoint(new Vector3(1, -GridStep));

        Vector3 leftUp = screenMatrix.MultiplyPoint(new Vector3(-GridStep, 1));
        Vector3 leftDown = screenMatrix.MultiplyPoint(new Vector3(-GridStep, -1));
        Vector3 rightUp = screenMatrix.MultiplyPoint(new Vector3(2 / 6f, 1));
        Vector3 rightDown = screenMatrix.MultiplyPoint(new Vector3(2 / 6f, -1));
        
        Gizmos.DrawLine(upLeft, upRight);
        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(leftUp, leftDown);
        Gizmos.DrawLine(rightUp, rightDown);
    }
}

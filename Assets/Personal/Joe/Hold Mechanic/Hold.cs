using Input;
using Interactable;
using Oscillators;
using Unity.VisualScripting;
using UnityEngine;

public class Hold : MonoBehaviour
{
    [SerializeField] private Camera _firstPersonCamera;
    [SerializeField] private float _reach;
    [SerializeField] private Transform _holdPivot;
    [SerializeField] private Collider _collider;
    
    private bool _didHit;
    private RaycastHit _hit;
    private Vector3 _velocity;
    private Vector3 _previousPosition = Vector3.zero;
    private Holdable _holding;
    
    private void FixedUpdate()
    {
        // TODO: Move raycast to it's own script
        _didHit = Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.TransformDirection(Vector3.forward), out _hit, _reach);
        
        if (!_holding)
        {
            // Begin holding, if conditions are appropriate
            if (!_didHit) return;
            var holdable = _hit.transform.GetComponent<Holdable>();
            if (!holdable) return;
            Hover();
            if (InputManager.Instance.GetButtonInput(ButtonMapping.Interact) != ButtonState.Hold) return;
            Grab(holdable);
        }
        else
        {
            if (InputManager.Instance.GetButtonInput(ButtonMapping.Interact) != ButtonState.Hold) {Release(); return;}
            _velocity = CalculateVelocity();
        }
    }
    
    private Vector3 CalculateVelocity()
    {
        var position = _holding.transform.position;
        Vector3 deltaPosition = position - _previousPosition;
        _previousPosition = position;
        Vector3 velocity = deltaPosition / Time.fixedDeltaTime; // Kinematics. Velocity is the change-in-position over time.
        return velocity;
    }

    private void Hover()
    {
        // TODO: Set cursor to open grab hand
    }

    private void Grab(Holdable holdable)
    {
        _holding = holdable;
        
        Physics.IgnoreCollision(_holding.GetComponent<Collider>(), _collider, true);
        var holdingTransform = _holding.transform;
        holdingTransform.SetParent(_holdPivot);
        _holding.GetComponent<Rigidbody>().useGravity = false;
        _holding.AddComponent<Oscillator>();
        _holding.AddComponent<TorsionalOscillator>();
        
        // TODO: Set cursor to closed grab hand
    }

    private void Release()
    {
        _holding.transform.SetParent(null);
        var holdingRigidbody = _holding.GetComponent<Rigidbody>();
        holdingRigidbody.useGravity = true;
        holdingRigidbody.AddForce(VelocityToImpulse(_velocity), ForceMode.Impulse);
        Physics.IgnoreCollision(_holding.GetComponent<Collider>(), _collider, false);
        Destroy(_holding.GetComponent<Oscillator>());
        Destroy(_holding.GetComponent<TorsionalOscillator>());
        
        // TODO: Conserve rotational velocity on release
        // TODO: Set cursor to default
        
        _holding = null;
    }
    
    private Vector3 VelocityToImpulse(Vector3 velocity)
    {
        var holdingRigidbody = _holding.GetComponent<Rigidbody>();
        var initialMomentum = Vector3.zero;
        var finalMomentum = holdingRigidbody.mass * velocity;
        var deltaMomentum = finalMomentum - initialMomentum;
        var impulse = deltaMomentum;
        return impulse;
    }
}

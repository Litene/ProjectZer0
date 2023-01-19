using System;
using System.Collections;
using Input;
using Interactable;
using Oscillators;
using Unity.VisualScripting;
using UnityEngine;

public class Hold : MonoBehaviour
{
    [SerializeField] private Camera _firstPersonCamera;
    [SerializeField] private float _reach;
    [SerializeField] private Transform _holdPivot; // TODO: _holdPivot should appear static w.r.t. the vertical camera rotation

    private bool _didHit;
    private RaycastHit _hit;
    
    private Holdable _holding;

    // TODO: Raycast through crosshair to detect the first object in reach
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
        }
    }

    private void Hover()
    {
        // TODO: Set cursor to open grab hand
    }

    private void Grab(Holdable holdable)
    {
        _holding = holdable;
        
        var holdingTransform = _holding.transform;
        holdingTransform.SetParent(_holdPivot);
        _holding.GetComponent<Rigidbody>().useGravity = false;
        _holding.AddComponent<Oscillator>();
        
        // TODO: Temporarily ignore collisions with held object
        // TODO: Add the rotational oscillator component to _holding
        // TODO: Set cursor to closed grab hand
    }

    private void Release()
    {
        Destroy(_holding.GetComponent<Oscillator>());
        _holding.transform.SetParent(null);
        _holding.GetComponent<Rigidbody>().useGravity = true;
        _holding = null;

        // TODO: See why the object loses momentum when released? Is there any momentum in the first place?
        // TODO: Remove the rotational oscillator component to _holding
    }
}

using System;
using System.Collections;
using Input;
using Interactable;
using UnityEngine;

public class Hold : MonoBehaviour
{
    [SerializeField] private Camera _firstPersonCamera;
    [SerializeField] private float _reach;
    [SerializeField] private Transform _holdPivot;

    private bool _didHit;
    private RaycastHit _hit;
    
    private Holdable _holding;

    // TODO: Raycast through crosshair to detect the first object in reach
    private void Update()
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
        holdingTransform.SetParent(_holdPivot); // TODO: Check if a rigidbody exists as a parent of _holdPivot, display an error if so.
        holdingTransform.localPosition = Vector3.zero;
        _holding.GetComponent<Rigidbody>().useGravity = false;
        // TODO: Add the oscillator component to _holding
        // TODO: Add the rotational oscillator component to _holding

        // TODO: Set cursor to closed grab hand
    }

    private void Release()
    {
        _holding.transform.SetParent(null);
        _holding.GetComponent<Rigidbody>().useGravity = true;
        _holding = null;
        // TODO: Remove the oscillator component to _holding
        // TODO: Remove the rotational oscillator component to _holding
        // TODO: See why the object loses momentum when released? Is there any momentum in the first place?
    }
}

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
            //if (InputManager.Instance.GetButtonInput(ButtonMapping.Interact) != ButtonState.Hold) {Release(); return;}
            _holding.GetComponent<Oscillator>().LocalEquilibriumPosition = _holdPivot.transform.position;
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
        //holdingTransform.SetParent(_holdPivot); // TODO: Check if a rigidbody exists as a parent of _holdPivot, display an error if so.
        //holdingTransform.localPosition = Vector3.zero;
        _holding.GetComponent<Rigidbody>().useGravity = false;
        _holding.AddComponent<Oscillator>();
        _holding.GetComponent<Oscillator>().LocalEquilibriumPosition = _holdPivot.transform.position;

        // TODO: Add the rotational oscillator component to _holding
        // TODO: Set cursor to closed grab hand
    }

    private void Release()
    {
        _holding.transform.SetParent(null);
        _holding.GetComponent<Rigidbody>().useGravity = true;
        _holding = null;
        // TODO: Remove the oscillator component from _holding
        //Destroy(_holding.GetComponent<Oscillator>());
        // TODO: Remove the rotational oscillator component to _holding
        // TODO: See why the object loses momentum when released? Is there any momentum in the first place?
    }
}

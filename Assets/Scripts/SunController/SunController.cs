using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SunController : MonoBehaviour {
    private GameObject _directionalLightObject;
    private Light _directionalLight;
    

    public LightState CurrentLightState = LightState.Night;
    public enum LightState { // make private
        Night,
        AstroTwilight,
        NauticalTwilight,
        CivilTwilight,
        DayLight
    }
    
    //public static void SetSunState

    private void Awake() {
        _directionalLightObject = GameObject.Find("Directional Light");
        _directionalLight = _directionalLightObject.GetComponent<Light>();
    }
}
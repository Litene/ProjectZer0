using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GC_WeatherTest : MonoBehaviour {
    private Light _dirLightObject;
    private GameObject _dirLight;
    private Transform _center;
    [SerializeField] [Range(1, 100)] private int timeMultiplier;
    
    public enum LightState { 
        Night,
        AstroTwilight,
        NauticalTwilight,
        CivilTwilight,
        DayLight
    }
    
    public LightState currentLightState = LightState.Night;
    private void Awake() {
        _dirLight = GameObject.Find("Directional Light");
        _dirLightObject = _dirLight.GetComponent<Light>();
        _center = GameObject.Find("Cube").transform;
    }
    
    
}
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
    private float currentTotalSeconds;

    public float CurrentTotalSeconds {
        get => currentTotalSeconds;
        set {
            currentTotalSeconds = value;
            if (currentTotalSeconds is 60 or > 60) {
                currentTotalSeconds = 0;
                CurrentTotalMinutes++;
            }
        }
    }
    
    private int currentTotalMinutes;

    private int CurrentTotalMinutes {
        get => currentTotalMinutes;
        set {
            currentTotalMinutes = value;
            if (currentTotalMinutes is 60 or > 60) {
                currentTotalMinutes = 0;
                CurrentHour++;
            }
        }
    }
    private int currentTotalHours;

    private int CurrentHour {
        get => currentTotalHours;
        set {
            currentTotalHours = value;
            if (currentTotalHours is 24 or > 24) {
                currentTotalHours = 0;
            }
        }
    }
    //[field: SerializeField]public float currentHourInMinutes => currentHours * 60;

    public enum LightState { // make private
        Night,
        AstroTwilight,
        NauticalTwilight,
        CivilTwilight,
        DayLight
    }

    //public static void ToLightProfile(LightState lightState) => lightState switch {
     //   LightState.Night =>

       //     _ => Debug.LogError("out of bounds;")
    //}

    public LightState currentLightState = LightState.Night;
    private void Awake() {
        _dirLight = GameObject.Find("Directional Light");
        _dirLightObject = _dirLight.GetComponent<Light>();
        _center = GameObject.Find("Cube").transform;
    }

    private void Update() {
        CurrentTotalSeconds += Time.deltaTime * timeMultiplier;
        Debug.Log($"{currentTotalHours}:{currentTotalMinutes}:{currentTotalSeconds}");
    }
}
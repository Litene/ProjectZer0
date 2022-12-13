using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.VFX;

[CustomEditor(typeof(SunController))] public class SunInspector : Editor {
    private SunController _controller;
    private void OnEnable() => _controller = target as SunController;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Night")) _controller.SetLight(LightState.Night);
        if (GUILayout.Button("Astro Twilight")) _controller.SetLight(LightState.AstroTwilight);
        if (GUILayout.Button("Nautical Twilight")) _controller.SetLight(LightState.NauticalTwilight);
        if (GUILayout.Button("Civil Twilight")) _controller.SetLight(LightState.CivilTwilight);
        if (GUILayout.Button("LowDayLight")) _controller.SetLight(LightState.LowDayLight);
        if (GUILayout.Button("DayLight")) _controller.SetLight(LightState.DayLight);
    }
}
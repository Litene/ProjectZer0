using System.Collections.Generic;
using UnityEngine;
public class SunController : MonoBehaviour {
    private readonly Dictionary<string, SunSetting> _sunSettings = new Dictionary<string, SunSetting>(); // exposed? I want headers..
    private Light _light;
    [ContextMenu("SetReferences")] private void Awake() {
        _light = transform.GetComponent<Light>();
        foreach (var sunSetting in Resources.LoadAll<SunSetting>("SunSettings")) {
            _sunSettings.Add(sunSetting.name,sunSetting);
        }
    }
    public void SetLight(LightState targetState) {
        if (_sunSettings.TryGetValue($"SunSetting{targetState}", out SunSetting lightState)) {
            transform.rotation = Quaternion.Euler(lightState.DegreesX, lightState.DegreesY, 0);
            _light.colorTemperature = lightState.ColorTemperature;
        }
    }
}
public enum LightState { 
    Night,
    AstroTwilight,
    NauticalTwilight,
    CivilTwilight,
    LowDayLight,
    DayLight
}

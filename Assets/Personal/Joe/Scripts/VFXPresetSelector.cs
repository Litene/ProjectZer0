using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// TODO: Make this an Editor class as opposed to MonoBehaviour (?)
[ExecuteAlways]
public class VFXPresetSelector : MonoBehaviour {
#if UNITY_EDITOR
    [SerializeProperty("SimpleSnowPreset")]
    public SimpleSnowPreset _simpleSnowPreset = SimpleSnowPreset.Light;
    public SimpleSnowPreset SimpleSnowPreset {
        get => _simpleSnowPreset;
        set {
            ApplySetting(_presetToSetting[value]);
            _simpleSnowPreset = value;
        }
    }

    private VisualEffect _visualEffect;
    private Dictionary<SimpleSnowPreset, SimpleSnowSetting> _presetToSetting = new Dictionary<SimpleSnowPreset, SimpleSnowSetting>();

    private void Awake() {
        _visualEffect = GetComponent<VisualEffect>();
    }

    private void OnEnable() {
        MapPresetsToSettings();
    }

    private void MapPresetsToSettings() {
        _presetToSetting[SimpleSnowPreset.Light] = new SimpleSnowSetting(Vector3.one * 10f, 16, 0.125f, 0.0f);
        _presetToSetting[SimpleSnowPreset.Moderate] = new SimpleSnowSetting(Vector3.one * 10f, 32, 0.25f, 0.5f);
        _presetToSetting[SimpleSnowPreset.Heavy] = new SimpleSnowSetting(Vector3.one * 10f, 64, 1f, 1.0f);
    }

    private void ApplySetting(SimpleSnowSetting simpleSnowSetting) {
        _visualEffect.SetVector3("Dimensions", simpleSnowSetting.Dimensions);
        _visualEffect.SetInt("Snowfall Rate", simpleSnowSetting.SnowfallRate);
        _visualEffect.SetFloat("Ambient Wind Strength", simpleSnowSetting.AmbientWindStrength);
        _visualEffect.SetFloat("Linear Wind Speed", simpleSnowSetting.LinearWindSpeed);
    }
#endif
}

public enum SimpleSnowPreset {
    Light,
    Moderate,
    Heavy
}

public struct SimpleSnowSetting {
    public Vector3 Dimensions;
    public int SnowfallRate;
    public float AmbientWindStrength;
    public float LinearWindSpeed;

    public SimpleSnowSetting(Vector3 dimensions, int snowfallRate, float ambientWindStrength, float linearWindSpeed) {
        Dimensions = dimensions;
        SnowfallRate = snowfallRate;
        AmbientWindStrength = ambientWindStrength;
        LinearWindSpeed = linearWindSpeed;
    }
}

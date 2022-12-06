using System;
using UnityEngine;

public class Preset : ScriptableObject {}

[CreateAssetMenu(menuName = "Presets/Simple Snow Preset")]
public class SimpleSnowPreset : Preset {
    public Vector3 Dimensions;
    [Range(0, 128)] public int SnowfallRate;
    [Range(0f, 1f)] public float AmbientWindStrength;
    [Range(0f, 1f)] public float LinearWindSpeed;

    public SimpleSnowPreset(Vector3 dimensions, int snowfallRate, float ambientWindStrength, float linearWindSpeed) {
        Dimensions = dimensions;
        SnowfallRate = snowfallRate;
        AmbientWindStrength = ambientWindStrength;
        LinearWindSpeed = linearWindSpeed;
    }
}

public enum SimpleSnowPresetMode {
    Light,
    Moderate,
    Heavy
}
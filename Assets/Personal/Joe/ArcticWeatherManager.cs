using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class ArcticWeatherController : MonoBehaviour {
    [SerializeField] [Range(0, 1)] private float _intensity;

    // TODO: Lerp the volume profile's "fog attenuation distance" from 100000 to 1000, based on _intensity.
    // TODO: Lerp snows values, based on _intensity.

    [SerializeField] private Volume _volume;
    [SerializeField] private VisualEffect _visualEffect;

    private readonly ExposedProperty SnowfallRateProperty = "Snowfall Rate";

    private Fog _fog;

    private void Awake() {
        _volume.profile.TryGet(out _fog);
    }
    
    private void SetFogProperties() {
        // Fog Attenuation Distance
        _fog.meanFreePath.value = 100000; 
    }

    // TODO: Check preset script that I made, as it may be useful here.
    private void SetSnowProperties() {
        _visualEffect.SetInt(SnowfallRateProperty, 0);
    }
}

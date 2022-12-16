using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

[ExecuteAlways]
public class ArcticWeatherController : MonoBehaviour {
    [SerializeProperty("Intensity")]
    public float _intensity;
    public float Intensity {
        get => _intensity;
        set {
            _intensity = Mathf.Clamp01(value);
            OnIntensityChanged?.Invoke(_intensity);
        }
    }
    
    [SerializeField] public UnityEvent<float> OnIntensityChanged;
    
    private const float TRANSITION_DURATION = 60f;

    private void Update()
    {
        if (!Application.isPlaying) { return; }
        
        Intensity += Time.deltaTime / TRANSITION_DURATION;
    }
}

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
}

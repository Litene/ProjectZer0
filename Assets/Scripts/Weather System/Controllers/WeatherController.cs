using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace WeatherSystem.Controllers {
    [ExecuteAlways]
    public class WeatherController : MonoBehaviour {
        [SerializeProperty("Intensity")]
        public float _intensity;
        public float Intensity {
            get => _intensity;
            set {
                if (value == _intensity) return;
                _intensity = Mathf.Clamp01(value);
                OnIntensityChanged?.Invoke(_intensity);
            }
        }

        [SerializeField] public UnityEvent<float> OnIntensityChanged;
    }
}

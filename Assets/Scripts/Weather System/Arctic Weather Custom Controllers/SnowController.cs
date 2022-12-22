using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace WeatherSystem
{
    [ExecuteAlways]
    public class SnowController : MonoBehaviour {
        private VisualEffect _snowVisualEffect;
        private readonly ExposedProperty _snowfallRateProperty = "Snowfall Rate";
        private readonly ExposedProperty _ambientWindStrengthProperty = "Ambient Wind Strength";
        [SerializeField] private AnimationCurve _intensityToSnowfallRateCurve;
        [SerializeField] private AnimationCurve _intensityToAmbientWindStrengthCurve;

        private void OnEnable() {
            _snowVisualEffect = GetComponent<VisualEffect>();
        }

        private int IntensityToSnowfallRate(float t) => (int)_intensityToSnowfallRateCurve.Evaluate(t);
        private float IntensityToAmbientWindStrength(float t) => _intensityToAmbientWindStrengthCurve.Evaluate(t);
        
        public void SetSnowProperties(float t) {
            _snowVisualEffect.SetInt(_snowfallRateProperty, IntensityToSnowfallRate(t));
            _snowVisualEffect.SetFloat(_ambientWindStrengthProperty, IntensityToAmbientWindStrength(t));
        }
    }
}

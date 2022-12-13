using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[ExecuteAlways]
public class SnowController : MonoBehaviour {
    private VisualEffect _snowVisualEffect;
    private readonly ExposedProperty SnowfallRateProperty = "Snowfall Rate";
    [SerializeField] private AnimationCurve _intensityToSnowfallRateCurve;

    private void OnEnable() {
        _snowVisualEffect = GetComponent<VisualEffect>();
    }

    private int IntensityToSnowfallRate(float t) => (int)_intensityToSnowfallRateCurve.Evaluate(t);

    // TODO: Check preset script that I made, as it may be useful here.
    public void SetSnowProperties(float t) {
        _snowVisualEffect.SetInt(SnowfallRateProperty, IntensityToSnowfallRate(t));
    }
}

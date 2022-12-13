using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class FogController : MonoBehaviour {
    private Volume _fogVolume;
    private Fog _fog;
    [SerializeField] private AnimationCurve _intensityToFogAttenuationDistanceCurve;

    private void OnEnable() {
        _fogVolume = GetComponent<Volume>();
        _fogVolume.profile.TryGet(out _fog);
    }

    private float IntensityToFogAttenuationDistance(float t) => _intensityToFogAttenuationDistanceCurve.Evaluate(t);

    public void SetFogProperties(float t) {
        _fog.meanFreePath.value = IntensityToFogAttenuationDistance(t); // Fog Attenuation Distance
    }
}

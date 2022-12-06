using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// TODO: Make VFXPresetSelector abstract (either an INTERFACE or abstract class...?)
// TODO: Make this an Editor class as opposed to MonoBehaviour (?)
[ExecuteAlways]
public class VFXPresetSelector : MonoBehaviour {
#if UNITY_EDITOR
    [SerializeProperty("SimpleSnowPresetMode")]
    public SimpleSnowPresetMode _simpleSnowPresetMode = SimpleSnowPresetMode.Light;
    public SimpleSnowPresetMode SimpleSnowPresetMode {
        get => _simpleSnowPresetMode;
        set {
            ApplySetting(PresetToSetting(value));
            _simpleSnowPresetMode = value;
        }
    }

    private VisualEffect _visualEffect;
    private Dictionary<SimpleSnowPresetMode, SimpleSnowPreset> _modeToPreset = new Dictionary<SimpleSnowPresetMode, SimpleSnowPreset>();
    private SimpleSnowPreset PresetToSetting(SimpleSnowPresetMode preset) => _modeToPreset[preset];
    
    private void Awake() {
        _visualEffect = GetComponent<VisualEffect>();
    }

    private void OnEnable() {
        MapPresetsToSettings();
    }

    private void MapPresetsToSettings() {
        /*
        _modeToPreset[SimpleSnowPresetMode.Light] = new SimpleSnowPreset(Vector3.one * 10f, 16, 0.125f, 0.0f);
        _modeToPreset[SimpleSnowPresetMode.Moderate] = new SimpleSnowPreset(Vector3.one * 10f, 32, 0.25f, 0.5f);
        _modeToPreset[SimpleSnowPresetMode.Heavy] = new SimpleSnowPreset(Vector3.one * 10f, 64, 1f, 1.0f);
        */
        
        // TODO: Use Adressables to access the scriptable objects...
        // https://docs.unity3d.com/Packages/com.unity.addressables@0.3/manual/AddressableAssetsGettingStarted.html
    }

    private void ApplySetting(SimpleSnowPreset simpleSnowPreset) {
        _visualEffect.SetVector3("Dimensions", simpleSnowPreset.Dimensions);
        _visualEffect.SetInt("Snowfall Rate", simpleSnowPreset.SnowfallRate);
        _visualEffect.SetFloat("Ambient Wind Strength", simpleSnowPreset.AmbientWindStrength);
        _visualEffect.SetFloat("Linear Wind Speed", simpleSnowPreset.LinearWindSpeed);
    }
#endif
}

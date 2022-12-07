using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        StartCoroutine(MapPresetsToSettings());
    }

    public List<string> keys = new List<string>() {"Light", "Moderate", "Heavy"};
    AsyncOperationHandle<IList<GameObject>> loadHandle;
    
    private IEnumerator MapPresetsToSettings() {
        /*
        _modeToPreset[SimpleSnowPresetMode.Light] = new SimpleSnowPreset(Vector3.one * 10f, 16, 0.125f, 0.0f);
        _modeToPreset[SimpleSnowPresetMode.Moderate] = new SimpleSnowPreset(Vector3.one * 10f, 32, 0.25f, 0.5f);
        _modeToPreset[SimpleSnowPresetMode.Heavy] = new SimpleSnowPreset(Vector3.one * 10f, 64, 1f, 1.0f);
        */
        
        /*
        // TODO: Use Addressables to access the scriptable objects...
        // https://docs.unity3d.com/Packages/com.unity.addressables@0.3/manual/AddressableAssetsGettingStarted.html
        Addressables.LoadAssetAsync<SimpleSnowPreset>("Assets/Personal/Joe/Scripts/Presets/Light.asset");
        Addressables.LoadAssetAsync<SimpleSnowPreset>("Assets/Personal/Joe/Scripts/Presets/Moderate.asset");
        Addressables.LoadAssetAsync<SimpleSnowPreset>("Assets/Personal/Joe/Scripts/Presets/Heavy.asset");

        var i = 0;
        loadHandle = Addressables.LoadAssetsAsync<SimpleSnowPreset>(
            keys,
            addressable =>
            {
                //Gets called for every loaded asset
                _modeToPreset[] = addressable;
                i++;
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail and release if any asset fails to load

        yield return loadHandle;
        */
        
        // TODO: Maybe enums aren't the way... I want to dynamically create a list of Presets based on a list of assets which are created.
        // TODO: I therefore need to first see if I can find all assets of type SimpleSnowPreset.

        yield return null;
    }

    private void ApplySetting(SimpleSnowPreset simpleSnowPreset) {
        _visualEffect.SetVector3("Dimensions", simpleSnowPreset.Dimensions);
        _visualEffect.SetInt("Snowfall Rate", simpleSnowPreset.SnowfallRate);
        _visualEffect.SetFloat("Ambient Wind Strength", simpleSnowPreset.AmbientWindStrength);
        _visualEffect.SetFloat("Linear Wind Speed", simpleSnowPreset.LinearWindSpeed);
    }
#endif
}

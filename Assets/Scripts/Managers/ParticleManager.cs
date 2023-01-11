using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager> {    
    private Dictionary<string, ParticleEffect> _availableEffects = new();
    private Dictionary<string, ParticleEffectPool> _pooledEffects = new();
    private void Awake() {
        ParticleEffect[] effects = Resources.LoadAll<ParticleEffect>("ParticleEffects");
        foreach (var effect in effects) {
            _availableEffects.Add(effect.name, effect);
        }
    }

    public void SpawnParticleEffect(string key, Vector3 position, Quaternion rotation, float playDelay = 0) {
        if (!_pooledEffects.ContainsKey(key)) {
            if(!_availableEffects.ContainsKey(key)) return;
            var effect = _availableEffects[key];

            var newPool = new GameObject($"EffectPool: {key}").AddComponent<ParticleEffectPool>();
            newPool.Init(effect); // we could feed the max particles here. 
        
            _pooledEffects.Add(key, newPool);
        }

        ParticleEffect particleEffect = _pooledEffects[key].Get();
        particleEffect.SetTransform(position, rotation);
        particleEffect.PlayDelay(playDelay);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager> {    
    private Dictionary<string, ParticleEffect> _storedEffects = new();
    private Dictionary<string, ParticleEffectPool> _activeEffects = new();
    private void Awake() {
        ParticleEffect[] effects = Resources.LoadAll<ParticleEffect>("ParticleEffects");
        foreach (var effect in effects) {
            _storedEffects.Add(effect.name, effect);
        }
    }

    public void SpawnParticleEffect(string key, Vector3 position, Quaternion rotation, float playDelay = 0) {
        if (!_activeEffects.ContainsKey(key)) {
            if(!_storedEffects.ContainsKey(key)) return;
            var effect = _storedEffects[key];

            var newPool = new GameObject($"EffectPool: {key}").AddComponent<ParticleEffectPool>();
            newPool.Init(effect);
        
            _activeEffects.Add(key, newPool);
        }

        ParticleEffect particleEffect = _activeEffects[key].Get();
        particleEffect.SetTransform(position, rotation);
        particleEffect.PlayDelay(playDelay);
    }
}
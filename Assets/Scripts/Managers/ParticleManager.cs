using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager> {
    public ParticleEffect testEffect;
    
    //todo populate _availableEffects on awake
    private void Awake() {
        _availableEffects.Add("test", testEffect);
        
        SpawnParticleEffect("test", Vector3.zero, Quaternion.identity, .5f);
    }

    private Dictionary<string, ParticleEffect> _availableEffects = new();
    private Dictionary<string, ParticleEffectPool> _pooledEffects = new();

    public void SpawnParticleEffect(string key, Vector3 position, Quaternion rotation, float playDelay = 0) {
        if (!_pooledEffects.ContainsKey(key)) {
            if(!_availableEffects.ContainsKey(key)) return;
            var effect = _availableEffects[key];

            var newPool = new GameObject($"EffectPool: {key}").AddComponent<ParticleEffectPool>();
            newPool.Init(effect);
        
            _pooledEffects.Add(key, newPool);
        }

        ParticleEffect particleEffect = _pooledEffects[key].Get();
        particleEffect.SetTransform(position, rotation);
        particleEffect.PlayDelay(playDelay);
    }
}

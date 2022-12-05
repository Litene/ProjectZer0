using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public ParticleEffect effect;
    
    private void Awake() {
        _availableEffects.Add("test", effect);
        
        SpawnParticleEffect("test", Vector3.zero, Quaternion.identity, .5f);
    }

    private Dictionary<string, ParticleEffect> _availableEffects = new Dictionary<string, ParticleEffect>();
    private Dictionary<string, ParticleEffectPool> _pooledEffects = new Dictionary<string, ParticleEffectPool>();

    public void SpawnParticleEffect(string key, Vector3 position, Quaternion rotation, float playDelay = 0) {
        Debug.Log("Spawn");
        
        if (!_pooledEffects.ContainsKey(key) && _availableEffects.ContainsKey(key)) {
            var effect = _availableEffects[key];

            var newPool = new GameObject("EffectPool").AddComponent<ParticleEffectPool>();
            newPool.Init(effect);
        
            _pooledEffects.Add(key, newPool);
        }
        else if(!_pooledEffects.ContainsKey(key) && !_availableEffects.ContainsKey(key)) return;
        
        ParticleEffect particleEffect = _pooledEffects[key].Get();
        
        particleEffect.SetTransform(position, rotation);
        particleEffect.PlayDelay(playDelay);
    }
}

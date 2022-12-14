using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffectPool : MonoBehaviour, IObjectPool<ParticleEffect> {
    private IObjectPool<ParticleEffect> _pool;
    private ParticleEffect _shaderParticleEffects;
    
    public ParticleEffect Get() => _pool.Get();
    public PooledObject<ParticleEffect> Get(out ParticleEffect v) => _pool.Get(out v);
    public void Release(ParticleEffect element) => _pool.Release(element);
    public void Clear() => _pool.Clear();
    public int CountInactive => _pool.CountInactive;
    public void Init(ParticleEffect shaderParticleEffects) {
        _shaderParticleEffects = shaderParticleEffects;
        _pool = new ObjectPool<ParticleEffect>(CreatePooledEffect, OnPoolGet, OnPoolRelease, OnPoolDestroy);
    }
    private ParticleEffect CreatePooledEffect() {
        var effect = Instantiate(_shaderParticleEffects, Vector3.zero, Quaternion.identity);
        effect.Init(_pool);
        return effect;
    }
    private static void OnPoolRelease(ParticleEffect effect) => effect.gameObject.SetActive(false);
    private static void OnPoolGet(ParticleEffect effect) => effect.gameObject.SetActive(true);
    private static void OnPoolDestroy(ParticleEffect effect) => Destroy(effect.gameObject);
}

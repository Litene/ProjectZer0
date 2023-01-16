using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class ParticleEffect : MonoBehaviour {
    private enum ParticleSystemType{ParticleSystem, VisualEffectsGraph}

    private ParticleSystemType _type;
    [SerializeField, Min(0)] private float _lifeTime = 0;
    [SerializeField, Min(0)] private int _maxEffects = 0;
    [SerializeField, Min(0)] private int _preSpawnAmount = 0;
    
    private IObjectPool<ParticleEffect> _pool;
    private ParticleSystem _particleSystem;
    private VisualEffect _visualEffect;
    
    public int MaxEffects => _maxEffects;
    public int PreSpawnAmount => _preSpawnAmount;

    private void OnParticleSystemStopped() => _pool.Release(this);

    public void Init(IObjectPool<ParticleEffect> objectPool) {
        _pool = objectPool;
        if (TryGetComponent(out _particleSystem)) {
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
            _type = ParticleSystemType.ParticleSystem;
        }
        else if (TryGetComponent(out _visualEffect)) {
            _type = ParticleSystemType.VisualEffectsGraph;
        }
    }
    
    public void SetTransform(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
    public void PlayDelay(float delay) => Invoke(nameof(Play), delay);
    private void Play() {
        if(_particleSystem == null && _visualEffect == null)
            return;
        
        switch (_type) {
            case ParticleSystemType.ParticleSystem: _particleSystem.Play(); break;
            case ParticleSystemType.VisualEffectsGraph: _visualEffect.Play(); break;
        }

        if(_lifeTime != 0) Invoke(nameof(OnParticleSystemStopped), _lifeTime);
    }
}
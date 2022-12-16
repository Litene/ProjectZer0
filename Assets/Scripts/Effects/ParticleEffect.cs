using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class ParticleEffect : MonoBehaviour {
    private enum ParticleSystemType{ParticleSystem, VisualEffectsGraph}

    private ParticleSystemType _type;
    [SerializeField, Min(0)] private float _lifeTime = 0;
    
    private IObjectPool<ParticleEffect> _pool;
    private ParticleSystem _particleSystem;
    private VisualEffect _visualEffect;
    
    private void OnParticleSystemStopped() => _pool.Release(this);

    public void Init(IObjectPool<ParticleEffect> objectPool) {
        _pool = objectPool;
        if (TryGetComponent(out ParticleSystem particleSystem)) {
            _particleSystem = particleSystem;
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
            
            _type = ParticleSystemType.ParticleSystem;
        }
        else if (TryGetComponent(out VisualEffect visualEffect)) {
            _visualEffect = visualEffect;

            _type = ParticleSystemType.VisualEffectsGraph;
        }
    }
    
    public void SetTransform(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
    public void PlayDelay(float delay) => Invoke(nameof(Play), delay);
    private void Play() {
        switch (_type) {
            case ParticleSystemType.ParticleSystem: _particleSystem.Play(); break;
            case ParticleSystemType.VisualEffectsGraph: _visualEffect.Play(); break;
        }

        if(_lifeTime != 0) Invoke(nameof(OnParticleSystemStopped), _lifeTime);
    }
}

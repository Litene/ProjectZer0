using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class ParticleEffect : MonoBehaviour {
    private enum ParticleSystemType{ParticleSystem, VisualEffectsGraph}

    private ParticleSystemType _type;
    private bool _isPlaying;
    
    private IObjectPool<ParticleEffect> _pool;

    private ParticleSystem _particleSystem;
    private VisualEffect _visualEffect;
    public void SetPool(IObjectPool<ParticleEffect> objectPool) => _pool = objectPool;

    private void OnParticleSystemStopped() {
        _isPlaying = false;
        _pool.Release(this);
    }

    public void Init() {
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

    private void LateUpdate() {
        if (_type != ParticleSystemType.VisualEffectsGraph) return;
        
        if(_visualEffect.aliveParticleCount == 0 && _isPlaying) OnParticleSystemStopped();
    }

    public void SetTransform(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
    public void PlayDelay(float delay) => Invoke(nameof(Play), delay);

    private void Play() {
        if (_type == ParticleSystemType.ParticleSystem) _particleSystem.Play();
        else if (_type == ParticleSystemType.VisualEffectsGraph) _visualEffect.Play();

        _isPlaying = true;
    }
}

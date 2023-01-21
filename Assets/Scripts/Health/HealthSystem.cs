using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.Rendering;

namespace Health {
    public class HealthSystem {
        private float _coldHealth;
        private float _health;
        private Material _coldMat;
        private Material _blackMat;
        private float _smoothTime = 0.5f;

        public HealthSystem(Material coldMat, Material blackMat) {
            _coldHealth = 22;
            _health = 150;
            _coldMat = coldMat;
            _blackMat = blackMat;
            _health = Mathf.Clamp(_health, 0f, 150f);
            _coldHealth = Mathf.Clamp(_coldHealth, 0f, 22f);
        }

        private float ClampHealth(float health) => Mathf.Clamp(health, 0, 150);
        private float ClampColdHealth(float coldHealth) => Mathf.Clamp(coldHealth, 0, 22);


        //todo: cold health regens lerp, blackout damage slerps, but lerps back. (probably)
        
        public void TakeDamage(Damage dmg, IDamagable self) {
            if (self.Health._health == 0 || self.Health._coldHealth == 0) return;
            if (dmg.DmgType == DamageType.Cold) {
                _coldHealth = ClampColdHealth(_coldHealth - dmg.GetDamage());
                _coldMat.SetFloat("_Sides", _coldHealth);
            }
            else {
                FadeBlack(_health, ClampHealth(_health - dmg.GetDamage()));
                _health = ClampHealth(_health - dmg.GetDamage());
            }

            if (_health == 0)  Blackout(self);
            else if (_coldHealth == 0) Whiteout(self);
        }

        public void RegenHealth(IDamagable self, float value) {
            self.Health._health = ClampHealth(value + self.Health._health);
            _blackMat.SetFloat("_Sides", self.Health._health);
        }

        public float RegenCold(IDamagable self, float value) {
            self.Health._coldHealth = ClampColdHealth(value + self.Health._coldHealth);
            _coldMat.SetFloat("_Sides", self.Health._coldHealth);
            return self.Health._coldHealth;
        }


        private async void FadeBlack(float initial, float target) {
            while (Mathf.Abs(_blackMat.GetFloat("_Sides") - target) <= 0.2f) {
                _blackMat.SetFloat("_Sides", Mathf.Lerp(initial, target, Time.deltaTime * _smoothTime));
            }

            _blackMat.SetFloat("_Sides", target);
        }

        private async void Whiteout(IDamagable self) { // Async method probably
            var whiteness = 0.9f;
            while (_coldMat.GetFloat("_Whiteness") <= 2) {
                _coldMat.SetFloat("_Whiteness", whiteness+=0.6f*Time.deltaTime);
                await Task.Yield();
            }
            Die(self);
        }

        private void Blackout(IDamagable self) { // Async method probably
            // needs to call die
        }
        
        private void Die(IDamagable self) {
            //implement
            // should call lots of stuff
            SceneLoader.Instance.ColdDeathTransition(_coldMat);
        }
    }

    public interface IDamager {
        Damage Dmg { get; set; }
    }

    public interface IDamagable {
        public float HealthRegen { get; set; }
        public float ColdRegen { get; set; }
        public float RegenHealthDelay { get; set; }
        public float RegenColdDelay { get; set; }
        public HealthSystem Health { get; set; }
        public Transform IDamagableTf { get; set; }
    }

    [System.Serializable]
    public class Damage { 
        [SerializeField] private float Value;
        [SerializeField] private float DamagePerSecond;
        public DamageType DmgType;
        public HealthLossType HealthType;
        public float GetDamage() => HealthType == HealthLossType.Instant ? Value : DamagePerSecond;
    }

    public enum DamageType {
        Cold,
        Blunt,
        Shock,
        Sharp,
        Poison
    }

    public enum HealthLossType {
        DamagePerSecond,
        Instant
    }
}
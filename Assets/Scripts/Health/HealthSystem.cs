using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

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
        }


        //22 is full health.
        //0 health 0
        //150 full health
        //0 health is 0
        public void TakeDamage(Damage dmg, IDamagable self) { // Invoke repeating, 
            if (dmg.DmgType == DamageType.Cold) {
                _coldHealth -= dmg.Value;
                _coldMat.SetFloat("Sides", _coldHealth);
            }
            else {
                FadeBlack(_health, _health - dmg.Value);
                _health -= dmg.Value;
            }

            if (_health <= 0) {
                _health = 0;
                Blackout(self);
            }
            else if (_coldHealth <= 0) {
                _coldHealth = 0;
                Whiteout(self);
            }

        }

        private async void FadeBlack(float initial, float target) {
            while (Mathf.Abs(_blackMat.GetFloat("Sides") - target) <= 0.2f) {
                _blackMat.SetFloat("Sides", Mathf.Lerp(initial, target, Time.deltaTime * _smoothTime));
            }
            _blackMat.SetFloat("Sides", target);
        }

        private void Whiteout(IDamagable self) {
            throw new System.NotImplementedException();
        }

        private void Blackout(IDamagable self) {
            throw new System.NotImplementedException();
        }
    }

    public interface IDamager {
        Damage Dmg { get; set; }
    }

    public interface IDamagable {
        public HealthSystem Health { get; set; }
    }

    [System.Serializable] public class Damage {
        public int Value;
        public DamageType DmgType;
        public HealthLossType HealthLossType;
        public float DamagePerSecond;
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
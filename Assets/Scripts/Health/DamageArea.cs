using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

[RequireComponent(typeof(Collider))]
public class DamageArea : MonoBehaviour {
    public Damage damage;
    public IDamagable IDamagable;
    private Collider _collider;

    private void Awake() {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Player player))
            if (player is IDamagable)  IDamagable = player;

        if (IDamagable != null) {
            InvokeRepeating(nameof(Damage), 0, 1 / damage.GetDamage());
            CancelInvoke(nameof(HealthCold));
        }
    }

    private void OnTriggerExit(Collider other) {
        CancelInvoke(nameof(Damage));
        RegenColdDelay();
    }

    private void RegenColdDelay() {
        if (!_collider.bounds.Contains(IDamagable.IDamagableTf.position)) {
            InvokeRepeating(nameof(HealthCold), IDamagable.RegenColdDelay, IDamagable.ColdRegen);
            var currentColdHealth = IDamagable.Health.RegenCold(IDamagable, IDamagable.ColdRegen);
            if (currentColdHealth == 22) {
                CancelInvoke(nameof(HealthCold));
                IDamagable = null;
            }
        }
        else CancelInvoke(nameof(HealthCold));
    }

    public void HealthCold() {
        IDamagable.Health.RegenCold(IDamagable, IDamagable.ColdRegen);
    }

    public void Damage() {
        IDamagable.Health.TakeDamage(damage, IDamagable);
    }
}
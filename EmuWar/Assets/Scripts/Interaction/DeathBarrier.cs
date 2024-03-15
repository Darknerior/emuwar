using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

/// <summary>
/// Deals damage to gameobjs inside.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class DeathBarrier : MonoBehaviour {
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickInterval = 1f;
    private List<GameObject> damageables = new();
    private Coroutine coroutine;
    private BoxCollider boxCollider;

    private void Start() {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == null)return;
        if(other.gameObject.TryGetComponent(out IDamageable _))damageables.Add(other.gameObject);
        if(damageables.Count >= 1 && coroutine == null )coroutine = StartCoroutine(ApplyDamageOverTime());
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject == null)return;
        if(other.gameObject.TryGetComponent(out IDamageable _) && damageables.Contains(other.gameObject))damageables.Remove(other.gameObject);
        if(damageables.Count < 1 && coroutine != null )coroutine = null;
    }
    
    
    /// <summary>
    /// Applies damage to idamageable game objects every tick
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyDamageOverTime() {
        while ( true ) {
            for (var i = damageables.Count - 1; i >= 0; i--) {
                var damageable = damageables[i];
                if (damageable != null && damageable.TryGetComponent(out IDamageable dmg)) dmg.TakeDamage(damagePerTick);
                else damageables.RemoveAt(i);
            }
            yield return new WaitForSeconds(tickInterval);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
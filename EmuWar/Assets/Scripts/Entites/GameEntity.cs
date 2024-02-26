using Interfaces;
using UnityEngine;

/// <summary>
/// Game entity is a parent class for a variety of object that have health and movement
/// </summary>
public class GameEntity : MonoBehaviour, IDamageable
{
    protected float health;
    protected float dmg;
    public float speed;
    
    /// <summary>
    /// Subtracts damage from health
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage) {
        health -= damage;
    }
    
    /// <summary>
    /// All entities should implement movement
    /// </summary>
    protected virtual void Movement() {
    }
    
    /// <summary>
    /// Deal damage to other entities
    /// </summary>
    /// <param name="damage"></param>
    protected void DealDamage(float damage) {
    }
    
    /// <summary>
    /// Death of entity, can be overridden for specific needs.
    /// </summary>
    protected void Die() {
        Destroy(gameObject);
    }
}

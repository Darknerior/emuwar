using System;
using System.Collections;
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
    protected float maxHealth;



    /// <summary>
    /// Subtracts damage from health
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage) {
        health -= damage;
        CheckHealth();
    }
    

    private void CheckHealth() {
        if(health <= 0)Die();
        if(health > maxHealth)health = maxHealth;
    }
    
    
    public float RemainingHealth => health;
    public float RemainingMaxHealth => maxHealth;

    public void Heal(float heals) {
        TakeDamage(-heals); 
        CheckHealth();
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
    protected virtual void Die() {
        Destroy(gameObject);
    }



}

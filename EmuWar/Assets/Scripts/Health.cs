using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHaveHealth
{
   [SerializeField] private int health;
    private int maxHealth;

    public void Start()
    {
        maxHealth = health;
    }
    public int RemainingHealth
    {
        get => health;
    }

    public void Heal(int heals)
    {
        TakeDamage(-heals); 
        CheckHealth();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Took Damage");
        health -= damage;
        Debug.Log($"{damage} {health}");
        CheckHealth();
    }

    public void CheckHealth()
    {
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

public interface IHaveHealth
{
    int RemainingHealth { get; }
    void TakeDamage(int damage);
    void Heal(int heals);
    void CheckHealth();
}

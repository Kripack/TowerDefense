using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;

    void Awake()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float damage) 
    {

    }
    public virtual void Die()
    {
        Debug.Log("Enemy died"); //для редактора
    }
}

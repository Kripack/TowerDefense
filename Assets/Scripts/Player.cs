using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] Animator animator;
    private bool isDead;
    void Start()
    {
        currentHP = maxHP;
    }
    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Отримано " + damage + " пошкодження. Залишилось " + currentHP + " ХП"); //для редактора
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("!GAME OVER!"); //для редактора
            animator.SetTrigger("Die");
        }
    }
}

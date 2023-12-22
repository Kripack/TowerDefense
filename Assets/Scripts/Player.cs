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
        Debug.Log("�������� " + damage + " �����������. ���������� " + currentHP + " ��"); //��� ���������
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("!GAME OVER!"); //��� ���������
            animator.SetTrigger("Die");
        }
    }
}

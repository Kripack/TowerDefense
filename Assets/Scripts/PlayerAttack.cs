using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovementPC playerMovement;
    [SerializeField] private float damage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask enemyLayer;

    private bool isAttack = false;
    public GameObject currentTarget; //public

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackDistance, enemyLayer);

        if (colliders != null && colliders.Length > 0)
        {
            // Перевірка, чи поточна ціль існує і ще є на дистанції атаки
            if (currentTarget == null || !IsWithinAttackDistance(currentTarget.transform.position))
            {
                currentTarget = GetClosestEnemy(colliders);
            }

            // Перевірка, чи поточна ціль все ще існує
            if (currentTarget != null && !isAttack)
            {
                int attackType = UnityEngine.Random.Range(1, 5);
                isAttack = true;
                playerMovement.CantMove();
                animator.Play(attackType.ToString());
            }
        }
        else
        {
            currentTarget = null;
        }
    }

    // Перевірка, чи об'єкт знаходиться на дистанції атаки
    bool IsWithinAttackDistance(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        return distance <= attackDistance;
    }

    // Отримання першого ворога зі списку ближніх ворогів
    GameObject GetClosestEnemy(Collider[] colliders)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            GameObject enemy = col.gameObject;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void OnDrawGizmosSelected()
    {
        // Малюємо сферу дистанції атаки для візуалізації
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }

    public void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamageable enemy))
            {
                enemy.TakeDamage(damage);
                Debug.Log("Нанесено " + damage + " пошкодження"); //для редактора
            }
        }
    }

    public void AttackRefresh()
    {
        isAttack = false;
        playerMovement.CanMove();
    }
}

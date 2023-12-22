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
            // ��������, �� ������� ���� ���� � �� � �� ��������� �����
            if (currentTarget == null || !IsWithinAttackDistance(currentTarget.transform.position))
            {
                currentTarget = GetClosestEnemy(colliders);
            }

            // ��������, �� ������� ���� ��� �� ����
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

    // ��������, �� ��'��� ����������� �� ��������� �����
    bool IsWithinAttackDistance(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        return distance <= attackDistance;
    }

    // ��������� ������� ������ � ������ ������ ������
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
        // ������� ����� ��������� ����� ��� ����������
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
                Debug.Log("�������� " + damage + " �����������"); //��� ���������
            }
        }
    }

    public void AttackRefresh()
    {
        isAttack = false;
        playerMovement.CanMove();
    }
}

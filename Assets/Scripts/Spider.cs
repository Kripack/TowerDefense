using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : Enemy
{
    [SerializeField] float damage;
    [SerializeField] private GameObject player;
    [SerializeField] private float viewDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerMask;
    private Animator animator;
    private SphereCollider collider;
    [SerializeField] private bool isAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        // Обертання на персонажа та атака
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < viewDistance)
        {
            Vector3 direction = -(player.transform.position - transform.position);
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
            if (!isAttack)
            {
                isAttack = true;
                int attackType = Random.Range(1, 3);
                animator.Play(attackType.ToString());
            }
        }
        //
    }
    public override void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
            return;
        }
        animator.Play("TakeDamage_002");
    }
    public void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, -attackPoint.forward, out hit, attackDistance, playerMask))
        {
            if (hit.collider.TryGetComponent(out IDamageable player))
            {
                player.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmos() // Для редактора
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(attackPoint.position, -attackPoint.forward * attackDistance);
    }
    public void AttackRefresh()
    {
        isAttack = false;
    }
    public override void Die()
    {
        animator.SetTrigger("Die");
        collider.enabled = false;
        viewDistance = 0;
        Debug.Log("Enemy died"); //для редактора
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

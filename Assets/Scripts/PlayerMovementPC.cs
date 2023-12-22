using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovementPC : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAttack playerAttack;

    private Rigidbody body;
    private bool canMove = true;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveInputX, 0f, moveInputZ);

        if ((moveInputX != 0 || moveInputZ != 0))
        {
            if (playerAttack.currentTarget != null)
            {
                Vector3 attackDirection = playerAttack.currentTarget.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(attackDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up).normalized;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            animator.SetBool("Moving", true);
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            body.velocity = movement;
        }
        else
        {
            animator.SetBool("Moving", false);
            body.velocity = Vector3.zero; // «упин€Їмо рух при зупинц≥ вводу
        }
    }

    public void CantMove()
    {
        canMove = false;
    }
    public void CanMove()
    {
        canMove = true;
    }
}

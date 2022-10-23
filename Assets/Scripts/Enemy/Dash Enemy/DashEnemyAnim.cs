using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyAnim : MonoBehaviour
{
    private EnemyCombat combat;
    private DashEnemyController controller;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        combat = GetComponentInParent<EnemyCombat>();
        controller = GetComponentInParent<DashEnemyController>();

    }

    private void OnEnable()
    {
        //controller.OnEnemyAttack += AttackAnim;
        combat.OnEnemyHealthDecrease += HitAnim;
        //combat.OnPlayerDeath += DeathAnim;
    }


    private void OnDisable()
    {
        //controller.OnEnemyAttack -= AttackAnim;
        combat.OnEnemyHealthDecrease -= HitAnim;
        //combat.OnPlayerDeath -= DeathAnim;
    }

    private void AttackAnim()
    {
       
    }

    private void Update()
    {
        IdleRunAnims();
    }

    private void IdleRunAnims()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
    }

    private void HitAnim()
    {
        animator.SetTrigger("TakeDamage");
    }

    //private void DeathAnim()
    //{
    //    animator.SetTrigger("Death");
    //}
    
}
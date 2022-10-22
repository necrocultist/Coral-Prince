using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAnim : MonoBehaviour
{
    private EnemyCombat combat;
    private RangedEnemyController controller;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        combat = GetComponentInParent<EnemyCombat>();
        controller = GetComponentInParent<RangedEnemyController>();
        
    }

    private void OnEnable()
    {
        controller.OnEnemyShoot += AttackAnim;
        combat.OnEnemyHealthDecrease += HitAnim;
        combat.OnPlayerDeath += DeathAnim;
    }


    private void OnDisable()
    {
        combat.OnPlayerHealthDecrease -= HitAnim;
        controller.OnPlayerAttack -= AttackAnim;
        combat.OnPlayerDeath -= DeathAnim;
    }

    private void Update()
    {
        IdleRunAnims();
    }

    private void IdleRunAnims()
    {
        animator.SetFloat("PlayerSpeed", Mathf.Abs(rigidBody.velocity.x));
    }

    private void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }
    private void HitAnim()
    {
        animator.SetTrigger("TakeDamage");
    }

    private void DeathAnim()
    {
        animator.SetTrigger("Death");
    }

    private void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
}
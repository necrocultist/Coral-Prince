using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //combat.OnPlayerDeath += DeathAnim;
    }


    private void OnDisable()
    {
        controller.OnEnemyShoot -= AttackAnim;
        combat.OnEnemyHealthDecrease -= HitAnim;
        //combat.OnPlayerDeath -= DeathAnim;
    }

    private void AttackAnim()
    {
        StartCoroutine(IEAttackAnim());
    }

    private void Update()
    {
        //IdleRunAnims();
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

    IEnumerator IEAttackAnim()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
    }
}
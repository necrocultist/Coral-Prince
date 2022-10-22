using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerCombat combat;
    private PlayerController controller;
    private Rigidbody2D rigidBody;
    private Animator animator;

    //[SerializeField] private float speedAnimOffset;

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        combat = GetComponentInParent<PlayerCombat>();
        controller = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        controller.OnPlayerJump += JumpAnim;
        controller.OnPlayerAttack += AttackAnim;
        controller.OnPlayerDash += DashAnim;
        combat.OnPlayerHealthDecrease += HitAnim;
        combat.OnPlayerDeath += DeathAnim;
    }


    private void OnDisable()
    {
        combat.OnPlayerHealthDecrease -= HitAnim;
        controller.OnPlayerAttack -= AttackAnim;
        controller.OnPlayerDash += DashAnim;
        combat.OnPlayerDeath -= DeathAnim;
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
    private void DashAnim()
    {
        animator.SetTrigger("Dash");
    }
}
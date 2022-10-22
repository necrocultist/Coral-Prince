using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerController controller;
    private Rigidbody2D rigidBody;
    private Animator animator;

    //[SerializeField] private float speedAnimOffset;

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        health = GetComponentInParent<PlayerHealth>();
        controller = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        health.OnPlayerHealthDecrease += HitAnim;
        controller.OnPlayerJump += JumpAnim;
        health.OnPlayerDeath += DeathAnim;
    }


    private void OnDisable()
    {
        health.OnPlayerHealthDecrease -= HitAnim;
        health.OnPlayerDeath -= DeathAnim;
        health.OnPlayerDeath -= DeathAnim;
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
}
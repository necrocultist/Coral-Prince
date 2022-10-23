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
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource dash;
    [SerializeField] private AudioSource death;

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
        jump.Play();
        animator.SetTrigger("Jump");
        //SoundManager.Instance.PlaySound(audioClips[3]);
    }
    private void HitAnim()
    {
        animator.SetTrigger("TakeDamage");
        //SoundManager.Instance.PlaySound(audioClips[0]);
    }

    private void DeathAnim()
    {
        death.Play();
        animator.SetTrigger("Death");
        //SoundManager.Instance.PlaySound(audioClips[2]);
    }

    private void AttackAnim()
    {
        attack.Play();
        animator.SetTrigger("Attack");
    }
    private void DashAnim()
    {
        dash.Play();
        animator.SetTrigger("Dash");
        //SoundManager.Instance.PlaySound(audioClips[1]);
    }
}
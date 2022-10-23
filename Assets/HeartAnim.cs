using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnim : MonoBehaviour
{
    private Animator animator;
    private EnemyCombat combat;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        combat = GetComponentInParent<EnemyCombat>();
    }

    private void OnEnable()
    {
        combat.OnEnemyHealthDecrease += HitAnim;

    }

    private void OnDisable()
    {
        combat.OnEnemyHealthDecrease += HitAnim;
    }

    private void HitAnim()
    {
        animator.SetTrigger("TakeDamage");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashEnemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    private Rigidbody2D enemyrb;
    private bool enemyAttack;
    [SerializeField] Animator animator;

    private bool isFacingRight = true;

    public event Action OnEnemyAttack;

    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyAttack = false;
        if (transform.position.x > player.transform.position.x && player != null)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else if(player != null)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        var dist = Vector2.Distance(transform.position, player.transform.position);
        
        if (attackRange < dist && dist< agroRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else if (dist < attackRange)
        {
            StartCoroutine(Timer());
        }
        else
        {
            enemyrb.velocity = Vector2.zero;
        }
    }
    public IEnumerator Timer()
    {
        animator.SetTrigger("Attack");
        int sign = isFacingRight ? 1 : -1;
        
        enemyrb.velocity = new Vector2(sign * dashSpeed, enemyrb.velocity.y);
        yield return new WaitForSeconds(0.5f);
        enemyrb.velocity = new Vector2(sign * moveSpeed, enemyrb.velocity.y);
        
    }
}


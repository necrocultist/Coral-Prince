using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;

    private Rigidbody2D enemyrb;
    
    public event Action OnEnemyShoot;


    public bool isFacingRight = true;
 
    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.x > player.transform.position.x)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
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
            OnEnemyShoot?.Invoke();
        }
        else
        {
            enemyrb.velocity = Vector2.zero;
        }
    }
    
}

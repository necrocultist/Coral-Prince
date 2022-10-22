using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] private float agroRange;

    private Rigidbody2D enemyrb;
    
    public event Action OnBossShoot;

    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var dist = Vector2.Distance(transform.position, player.transform.position);
    
        if (dist < agroRange)
        {
            OnBossShoot?.Invoke();
        }
    }
    
}


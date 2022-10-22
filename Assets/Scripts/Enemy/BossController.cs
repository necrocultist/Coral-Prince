using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Transform player;

    private Rigidbody2D enemyrb;

    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }
    
}


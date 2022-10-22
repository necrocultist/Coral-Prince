using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    public float lifeTime;
    public Transform nextBullet;
    Rigidbody2D rba;
    void Start()
    {
        rba = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {   
        float angle =  Mathf.Atan2(rba.velocity.y, rba.velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    }

}

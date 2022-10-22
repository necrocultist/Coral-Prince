using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private float nextFire = 0.5F;
    [SerializeField] private RangedEnemyController _enemyController;
    [SerializeField] private float bulletSpeed;

    private float myTime = 0.0F;
    private float fireDelta = 0.5F;
    
    private void OnEnable()
    {
        _enemyController.OnEnemyShoot += ShootBullets;
    }
    private void OnDisable()
    {
        _enemyController.OnEnemyShoot -= ShootBullets;
    }
    
    void ShootBullets()
    {
        myTime += Time.deltaTime;

        if (myTime > nextFire)
        {
            nextFire = myTime + fireDelta;

            float xSpeedSign = _enemyController.isFacingRight ? 1 : -1;

            GameObject tempBullet = null;
            var tempTempBullet = tempBullet;
            tempBullet = Instantiate(playerBullet, transform.position, Quaternion.identity);
            tempBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * bulletSpeed, ForceMode2D.Impulse);
            tempBullet.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(xSpeedSign * 1, 0) * bulletSpeed, ForceMode2D.Impulse);
            tempBullet.GetComponent<SpriteRenderer>().flipX = this.GetComponent<Transform>().localScale.x == -1;

            nextFire = nextFire - myTime;

            myTime = 0f;
        }
    }
}
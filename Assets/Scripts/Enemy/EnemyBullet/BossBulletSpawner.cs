using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private float nextFire = 0.5F;
    [SerializeField] private BossController _bossController;
    [SerializeField] private float bulletSpeed;

    private float myTime = 0.5F;
    private float fireDelta = 0.5F;
    
    private void OnEnable()
    {
        _bossController.OnBossShoot += ShootBullets;
    }
    private void OnDisable()
    {
        _bossController.OnBossShoot -= ShootBullets;
    }
    
    void ShootBullets()
    {
        
    }
}

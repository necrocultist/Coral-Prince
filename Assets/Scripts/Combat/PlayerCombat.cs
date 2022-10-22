using System;
using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    [SerializeField] private int enemyDamage;
    [SerializeField] private int playerDamage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackRange;

    private PlayerController controller;
    private Collider2D collider;

    public event Action OnPlayerHealthDecrease;
    public event Action OnPlayerDeath;
    public event Action OnPlayerAttack;

    private void OnEnable()
    {
        controller.OnPlayerAttack += Attack;
    }

    private void OnDisable()
    {
        controller.OnPlayerAttack -= Attack;
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        controller = GetComponent<PlayerController>();
    }

    public void DecraseHealth(int damage)
    {
        currentHealth -= damage;

        if (!AliveCheck())
        {
            OnPlayerDeath?.Invoke();
            Destroy(gameObject, 0.5f);
        }
        else
        {
            OnPlayerHealthDecrease?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject gameObject = other.gameObject;

        if (gameObject != null)
        {
            if (gameObject.TryGetComponent(out EnemyCombat _) || gameObject.TryGetComponent(out EnemyBulletMovement _))
            {
                DecraseHealth(enemyDamage);
            }
        }
        else
        {
            Debug.Log("There is no gameObject this" + name + "collides.");
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyCombat>().AliveCheck())
            {
                enemy.GetComponent<EnemyCombat>().DecraseHealth(playerDamage);
                //if (enemy.GetComponent<EnemyManager>().enemyAlive) knockbackManager.Knock(transform, enemy.transform, 0.01f, 55f);
            }
        }
    }
}
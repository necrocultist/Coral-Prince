using System;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public event Action OnEnemyHealthDecrease;

    public void DecraseHealth(int damage)
    {
        OnEnemyHealthDecrease?.Invoke();
        currentHealth -= damage;

        if (!AliveCheck())
        {
            Destroy(gameObject);
        }
    }
}
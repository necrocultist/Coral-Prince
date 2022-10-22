using System;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public event Action OnEnemyHealthDecrease;
   
    [SerializeField] private GameManager gm;

    public void DecraseHealth(int damage)
    {
        OnEnemyHealthDecrease?.Invoke();
        currentHealth -= damage;

        if (!AliveCheck())
        {
            if (this.gameObject.name == "Boss")
            {
                gm.currentState = States.Won;
            }
            Destroy(gameObject);
        }
    }
}
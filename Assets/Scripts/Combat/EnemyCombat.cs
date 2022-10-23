using System;
using System.Collections;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public event Action OnEnemyHealthDecrease;

    public Animator animator;
    [SerializeField] private GameManager gm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        WaitForMe();
    }

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

    IEnumerator WaitForMe()
    {
        yield return new WaitForSeconds(0.5f);
        
        animator.SetTrigger("DamageExit");
    }
}
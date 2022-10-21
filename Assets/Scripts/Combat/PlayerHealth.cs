using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerHealth : CharacterHealth
{
    [SerializeField] private int contactDamage;

    private Collider2D collider;

    public event Action OnPlayerHealthDecrease;
    public event Action OnPlayerDeath;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
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
            if (gameObject.TryGetComponent(out EnemyHealth _))
            {
                DecraseHealth(contactDamage);
            }
        }
        else
        {
            Debug.Log("There is no gameObject this" + name + "collides.");
        }
    }
}
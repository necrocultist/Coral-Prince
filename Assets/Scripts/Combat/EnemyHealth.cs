using UnityEngine;

public class EnemyHealth : CharacterHealth
{
    public void DecraseHealth(int damage)
    {
        currentHealth -= damage;

        if (!AliveCheck())
        {
            Destroy(gameObject);
        }
    }
}
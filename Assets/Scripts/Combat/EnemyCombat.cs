using UnityEngine;

public class EnemyCombat : CharacterCombat
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
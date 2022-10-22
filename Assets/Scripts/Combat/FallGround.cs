using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGround : MonoBehaviour
{
    [SerializeField] PlayerCombat _playerCombat;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        { 
            _playerCombat.DecraseHealth(500);
        }
    }
}

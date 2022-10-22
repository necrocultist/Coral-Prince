/*using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform castPoint;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    private Rigidbody2D enemyrb;

    private bool isFacingLeft;

    public event Action OnPlayerSpotted;
    public event Action OnAttack;

    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (CanSeePlayer(agroRange))
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (isFacingLeft)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "Player")
            {
                val = true;
            }
            else
            {
                val = false;
            }

            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.cyan);
        }

        return val;
    }

    private void StopChasingPlayer()
    {
        enemyrb.velocity = new Vector2(0, enemyrb.velocity.y);
    }

    private void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            enemyrb.velocity = new Vector2(moveSpeed, 0);
            isFacingLeft = false;
        }
        else
        {
            enemyrb.velocity = new Vector2(-moveSpeed, 0);
            transform.Rotate(0, 180, 0);
            isFacingLeft = true;
        }

        if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            enemyrb.velocity = new Vector2(0, enemyrb.velocity.y);
            StartCoroutine(Timer());
            
        }
    }

    IEnumerator Timer()
    {
        
        enemyrb.velocity = new Vector2(dashSpeed, enemyrb.velocity.y);
        yield return new WaitForSeconds(0.5f);
        enemyrb.velocity = new Vector2(moveSpeed, enemyrb.velocity.y);
    }
}*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform castPoint;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    private Rigidbody2D enemyrb;

    private bool isFacingRight = true;

    public event Action OnPlayerSpotted;
    public event Action OnAttack;

    private void Awake()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.x > player.transform.position.x)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        var dist = Vector2.Distance(transform.position, player.transform.position);
        
        if (attackRange < dist && dist< agroRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else if (dist < attackRange)
        {
            StartCoroutine(Timer());
        }
        else
        {
            enemyrb.velocity = Vector2.zero;
        }
    }
    IEnumerator Timer()
    {
        int sign = isFacingRight ? 1 : -1;
        
        enemyrb.velocity = new Vector2(sign * dashSpeed, enemyrb.velocity.y);
        yield return new WaitForSeconds(0.5f);
        enemyrb.velocity = new Vector2(sign * moveSpeed, enemyrb.velocity.y);
        
    }
}


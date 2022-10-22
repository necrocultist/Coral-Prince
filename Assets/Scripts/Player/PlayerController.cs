using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed;
    private float movementDirection;
    private bool facingRight = true;

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    [Header("Ground Check")]
    private bool isPlayerGrounded;
    [SerializeField] float groundCheckDistance;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private LayerMask groundCheckLayer;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool canDoubleJump = false;
    public bool doubleJumpMode = false;

    [Header("Physics")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float fallMultiplier = 4f;
    private Rigidbody2D rb;

    public event Action OnPlayerJump;
    public event Action OnPlayerAttack;
    public event Action OnPlayerDash;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(movementDirection);


        if (isDashing)
        {
            return;
        }
        else
        {
            ModifyPhysics();
        }

        if (!doubleJumpMode)
        {
            if (isPlayerGrounded && jumpTimer > Time.time) Jump();
        }
        else
        {
            if (isPlayerGrounded)
            {
                canDoubleJump = true;
            }
            if (jumpTimer > Time.time)
            {
                if (isPlayerGrounded)
                {
                    Jump();
                }
                else
                {
                    if (canDoubleJump)
                    {
                        Jump();
                        canDoubleJump = false;
                    }
                }
            }
        }


    }

    private void GetInput()
    {
        movementDirection = Input.GetAxisRaw("Horizontal");

        OnGroundCheck();

        if(Input.GetButtonDown("Dash") && canDash)
        {
            OnPlayerDash?.Invoke();
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Fire1") && canDash)
            OnPlayerAttack?.Invoke();
      }

    private void MovePlayer(float direction)
    {
        if ((direction * moveSpeed < 0 && facingRight) || (direction * moveSpeed > 0 && !facingRight)) FlipFace();

        //rb.AddForce(direction.x * moveSpeed * Vector2.right);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        //if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        //{
        //    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        //}
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(jumpSpeed * Vector2.up, ForceMode2D.Impulse);
        jumpTimer = 0;

        OnPlayerJump?.Invoke();
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float oldGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.AddForce(dashPower * Vector2.right, ForceMode2D.Impulse);
        //rb.velocity = new Vector2(transform.pos.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = oldGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void FlipFace()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    private void ModifyPhysics()
    {
        bool changingDirection = (movementDirection > 0 && rb.velocity.x < 0) || (movementDirection < 0 && rb.velocity.x > 0);

        if (OnGroundCheck())
        {
            if (Math.Abs(movementDirection) < 0.4f || changingDirection)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = linearDrag * 0.8f;
            }
            if (OnGroundCheck())
                rb.gravityScale = 0f;
            else
                rb.gravityScale = 4f;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag;

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    #region Checks and Gizmos
    private bool OnGroundCheck()
    {
        isPlayerGrounded =
            Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundCheckDistance,
                groundCheckLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down,
                groundCheckDistance, groundCheckLayer);
        return isPlayerGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + (Vector3.down * groundCheckDistance));
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + (Vector3.down * groundCheckDistance));
    }
    #endregion
}
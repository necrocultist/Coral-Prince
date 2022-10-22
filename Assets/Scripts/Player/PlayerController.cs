using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed;
    private float movementDirection;
    private bool facingRight = true;
    private bool canMove = true;
    private bool canFlip = true;

    [Header("Dash")]
    private bool isDashing;

    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float distanceBetweenImages = 0.1f;
    [SerializeField] private float dashCoolDown = 2.5f;
    private float dashTimeLeft;
    private float LastImageXpos;
    private float lastDash = -100;

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

        CheckDash();
    }

    private void FixedUpdate()
    {
        if (canMove)
            MovePlayer(movementDirection);



        ModifyPhysics();


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

        if (Input.GetButtonDown("Dash") && Time.time >= (lastDash + dashCoolDown))
        {
            OnPlayerDash?.Invoke();
            //StartCoroutine(Dash());
            Dash();
        }

        if (Input.GetButtonDown("Fire1") /*&& canDash*/)
            OnPlayerAttack?.Invoke();
    }

    private void Dash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        LastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;

                rb.velocity = new Vector2(dashSpeed * movementDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - LastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    LastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }


    private void MovePlayer(float direction)
    {
        if (((direction * moveSpeed < 0 && facingRight) || (direction * moveSpeed > 0 && !facingRight)) && canFlip)
            FlipFace();

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(jumpSpeed * Vector2.up, ForceMode2D.Impulse);
        jumpTimer = 0;

        OnPlayerJump?.Invoke();
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
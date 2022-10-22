using System;
using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 inputDir;
    private bool facingRight = true;
    private bool isDashing;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float distanceBetweenImages;
    [SerializeField] private float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(inputDir);

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
        inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        OnGroundCheck();

        Dash();
    }

    private void MovePlayer(Vector2 direction)
    {
        if ((direction.x * moveSpeed < 0 && facingRight) || (direction.x * moveSpeed > 0 && !facingRight)) FlipFace();

        rb.AddForce(direction.x * moveSpeed * Vector2.right);

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
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCooldown))
            {
                isDashing = true;
                dashTimeLeft = dashTime;
                lastDash = Time.time;

                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }
    }

    private void FlipFace()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    private void ModifyPhysics()
    {
        bool changingDirection = (inputDir.x > 0 && rb.velocity.x < 0) || (inputDir.x < 0 && rb.velocity.x > 0);

        if (OnGroundCheck())
        {
            if (Math.Abs(inputDir.x) < 0.4f || changingDirection)
            {
                rb.drag = linearDrag * 1.5f;
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
            else if (rb.velocity.y > 0 && (name == "Player1" && !Input.GetKey(KeyCode.W) || name == "Player2" && !Input.GetKey(KeyCode.UpArrow)))
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

    private void DashCheck()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(dashSpeed * inputDir.x * Time.deltaTime, rb.velocity.y);
            dashTimeLeft -= Time.deltaTime;

            if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }

            if(dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }

    #endregion
}
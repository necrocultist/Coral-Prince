using System;
using System.Collections;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator animator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    private float movementInputDirection;

    private bool isFacingRight = true;
    private bool isWalking;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool canDoubleJump = true;
    public bool doubleJumpMode = false;

    [Header("Ground Check")]
    [SerializeField] private bool isPlayerGrounded;
    [SerializeField] float groundCheckDistance;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private LayerMask groundCheckLayer;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        CheckMovementDirection();
        UpdateAnimations();

        if (Input.GetAxisRaw("Jump") != 0)
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }


    private void FixedUpdate()
    {
        ApplyMovement();

        if (!doubleJumpMode)
        {
            if (isPlayerGrounded && jumpTimer > Time.time) Jump();
        }
        else
        {
            if (isPlayerGrounded && jumpTimer > Time.time)
            {
                canDoubleJump = true;
                Jump();
                if (canDoubleJump)
                {
                    Jump();
                    canDoubleJump = false;
                }
            }
        }
    }

        private void ApplyMovement()
        {

            playerRB.velocity = new Vector2(movementSpeed * movementInputDirection * Time.deltaTime, playerRB.velocity.y);
        }

        private void GetInput()
        {
            movementInputDirection = Input.GetAxisRaw("Horizontal");
        }

        private void CheckMovementDirection()
        {
            if ((isFacingRight && movementInputDirection * movementSpeed < 0) || (!isFacingRight && movementInputDirection * movementSpeed > 0))
                Flip();

            if (Input.GetButtonDown("Jump"))
                Jump();

            if (playerRB.velocity.x != 0)
                isWalking = true;
            else
                isWalking = false;

            OnGroundCheck();
        }

        private void Jump()
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);

            playerRB.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            jumpTimer = 0;
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
        }

        private void UpdateAnimations()
        {
            animator.SetBool("isWalking", isWalking);
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

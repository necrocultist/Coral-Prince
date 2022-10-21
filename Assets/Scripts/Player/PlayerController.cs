using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;
    private bool isFacingRight;
    private Rigidbody2D playerRB;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        CheckMovementDirection();
    }


    private void FixedUpdate()
    {
        ApplyMovement();
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
        if ((isFacingRight && movementInputDirection * movementSpeed < 0) || (!isFacingRight && movementInputDirection * movementSpeed  > 0))
            Flip();

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce * Time.deltaTime);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }
}

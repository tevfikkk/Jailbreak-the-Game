using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Variables
    [SerializeField] float moveSpeed = 5.5f;
    [SerializeField] float jumpSpeed = 20f;

    // References 
    Vector2 moveInput;
    Rigidbody2D myRb;
    Animator myAnim;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myAnim = GetComponent<Animator>() as Animator;
        myBodyCollider = GetComponent<CapsuleCollider2D>() as CapsuleCollider2D;
        gravityScaleAtStart = myRb.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>() as BoxCollider2D;
    }


    void Update()
    {
        Run();
        FlipPlayer();
        ClimbLadder();
    }

    // Input System
    void OnMove(InputValue context)
    {
        moveInput = context.Get<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }

    void OnJump(InputValue context)
    {
        if (context.isPressed)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        myRb.velocity = new Vector2(myRb.velocity.x, jumpSpeed);
    }

    // Flip Player
    void FlipPlayer()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRb.velocity.x), 1f);
        }
    }

    // Run Method to move the player
    void Run()
    {
        bool isRunning = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRb.velocity.y);
        myRb.velocity = playerVelocity;

        myAnim.SetBool("isRunning", isRunning);

    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRb.gravityScale = gravityScaleAtStart;
            myAnim.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRb.velocity.x, moveInput.y * moveSpeed);
        myRb.velocity = climbVelocity;
        myRb.gravityScale = 0f;

        bool isClimbing = Mathf.Abs(myRb.velocity.y) > Mathf.Epsilon;
        myAnim.SetBool("isClimbing", isClimbing);
    }
}
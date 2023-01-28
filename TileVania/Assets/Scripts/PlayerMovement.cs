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
    bool isAlive = true;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

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
        if (!isAlive) { return; }
        Run();
        FlipPlayer();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue context)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    // Input System
    void OnMove(InputValue context)
    {
        if (!isAlive) { return; }
        moveInput = context.Get<Vector2>();
        // Debug.Log($"Move Input: {moveInput}");
    }

    void OnJump(InputValue context)
    {
        if (!isAlive) { return; }
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

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnim.SetTrigger("Dying");
            myRb.velocity = deathKick;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Variables
    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float jumpSpeed = 15f;
    float moveInput;
    bool isFacingRight = true;

    // References 
    Rigidbody2D myRb;
    Animator myAnim;
    CapsuleCollider2D myCollider;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myAnim = GetComponent<Animator>() as Animator;
        myCollider = GetComponent<CapsuleCollider2D>() as CapsuleCollider2D;
    }


    void Update()
    {
        if (moveInput > 0f && !isFacingRight)
        {
            FlipPlayer();
        }
        else if (moveInput < 0f && isFacingRight)
        {
            FlipPlayer();
        }
    }

    void FixedUpdate() => Run();

    // Input System
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        Debug.Log($"Move Input: {moveInput}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            myRb.velocity = new Vector2(myRb.velocity.x, jumpSpeed);
        }

        if (context.canceled && myRb.velocity.y > 0)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, myRb.velocity.y * 0.5f);
        }
    }

    bool IsGrounded() => myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    // Flip Player
    void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;

        myAnim.SetBool("isRunning", playerHasHorizontalSpeed);

        myRb.velocity = new Vector2(moveInput * moveSpeed, myRb.velocity.y);
    }
}

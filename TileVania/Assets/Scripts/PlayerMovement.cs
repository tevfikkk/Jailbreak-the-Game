using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Variables
    [SerializeField] float moveSpeed = 3.5f;

    // References 
    Vector2 moveInput;
    Rigidbody2D myRb;
    Animator myAnim;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myAnim = GetComponent<Animator>() as Animator;
    }


    void Update()
    {
        Run();
        FlipPlayer();
    }

    // Input System
    void OnMove(InputValue context)
    {
        moveInput = context.Get<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
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
}

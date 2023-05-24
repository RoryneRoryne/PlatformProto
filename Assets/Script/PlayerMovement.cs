using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D rgdbdy2D;
    Animator myAnimator;
    float gravityScaleAtStart;
   
    CapsuleCollider2D capsuleCollider;
    
    
    void Start()
    {
        rgdbdy2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rgdbdy2D.gravityScale;
    }

    
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        
        if (value.isPressed)
        {
            rgdbdy2D.velocity += new Vector2 (0f, jumpSpeed);
            // myAnimator.SetBool("isJumping",value: true);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rgdbdy2D.velocity.y); 
        rgdbdy2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rgdbdy2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rgdbdy2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rgdbdy2D.velocity.x),1f);
        }
        
    }

    void ClimbLadder()
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rgdbdy2D.gravityScale = gravityScaleAtStart;
            return;
        }
        Vector2 climbVelocity = new Vector2(rgdbdy2D.velocity.x, moveInput.y * climbSpeed);
        rgdbdy2D.velocity = climbVelocity;
        rgdbdy2D.gravityScale = 0f;
    }
}

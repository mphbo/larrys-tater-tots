using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float wallJumpSpeed = 2f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    bool hasWallJumped = false;
    bool hasDied = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Run(); 
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        if (!hasDied)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        bool isTouchingFeet = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isTouchingBody = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingFeet && !isTouchingBody)
        {
            return;
        }
        if (value.isPressed)
        {
            if (isTouchingFeet)
            {
                Jump();
                hasWallJumped = false;
            }
            else if (!hasWallJumped)
            {
                Debug.Log("localScale:", transform);
                WallJump();
                hasWallJumped = true;
            }
        }
        
    }

    void Jump()
    {
        if (!hasDied)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void WallJump()
    {
        myRigidbody.velocity += new Vector2(wallJumpSpeed, jumpSpeed);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite() 
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; 
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * 3.5f, 3.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        bool isTouchingBody = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        if (isTouchingBody)
        {
            Destroy(other.gameObject);
            hasDied = true;
        }     
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        bool isTouchingFeet = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        if (other.gameObject.tag == "Enemy" && isTouchingFeet)    
        {
            Jump();            
            Destroy(other.gameObject);
        }
    }
}

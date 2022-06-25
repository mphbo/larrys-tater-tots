using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCollider;
    Animator myAnimator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Run(); 
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        bool isTouching = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouching)
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
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
}

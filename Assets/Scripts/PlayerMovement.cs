using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float wallJumpSpeed = 2f;
    [SerializeField] float invincibleTime = 0.5f;
    float playerHealth = 2f;
    float baseSize = 2f;
    float sizeAdjust = 1.75f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    GameObject player;

    bool hasWallJumped = false;
    bool isInvincible = false;
    bool hasDied = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
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
        bool isTouchingHazard = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard"));
        bool isTouchingBody = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingFeet && !isTouchingBody && !isTouchingHazard)
        {
            return;
        }
        if (value.isPressed)
        {
            if (isTouchingFeet || isTouchingHazard)
            {
                Jump();
                hasWallJumped = false;
            }
            else if (!hasWallJumped)
            {
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
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * sizeAdjust * baseSize, sizeAdjust * baseSize);
        }
    }

    void Die()
    {
        Destroy(myBodyCollider);
        hasDied = true;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
        // todo I need to figure out an animation or programmatic spin for character when dying
    }

    void Shrink() 
    {
        transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * sizeAdjust * baseSize, sizeAdjust * baseSize);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        bool isTouchingEnemy = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        bool isTouchingHazard = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazard"));
        Debug.Log(isInvincible);
        if (isTouchingEnemy || isTouchingHazard)
        {
            if (isTouchingEnemy)
            {
                Destroy(other.gameObject);
            }
            myAnimator.SetTrigger("hit");
            if (playerHealth > 1)
            {
                Shrink();
                playerHealth -= 1;
                sizeAdjust = 1.25f;
                isInvincible = true;
                myAnimator.SetBool("isInvincible", true);
                StartCoroutine(MakeMortal());
            }
            else if (!isInvincible)
            {
                Die();
            }
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

    IEnumerator MakeMortal()
    {
        yield return new WaitForSeconds(invincibleTime);
        Debug.Log("hitMakeMortalTimeUp");
        isInvincible = false;
        myAnimator.SetBool("isInvincible", false);
    }
}

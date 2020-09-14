using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Animator animator = default;

    [SerializeField] float speed = 75;
    [SerializeField] float jumpForce = 1500;
    [SerializeField] float dashForce = 1;
    [SerializeField] float maxJumps = 1;
    [SerializeField] float currentJumps = 1;

    [SerializeField] Transform groundCheckPosition = default;
    [SerializeField] float groundCheckRadius = 0.05f;
    [SerializeField] LayerMask validLayers = default;
    [SerializeField] bool isGrounded = false;

    [SerializeField] SpriteRenderer sRenderer = default;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody for " + this.name + " not found");
    }

    bool jump = false;
    bool dash = false;
    float direction = 0;
    void Update()
    {
        Inputs();

        TempAttackMethodForTesting();
        animator.SetBool("Falling", !isGrounded);
    }

    void FixedUpdate()
    {
        Move();
        Dash();
        Jump();
    }
    
    void Inputs()
    {
        direction = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            jump = true;
        }
        if(Input.GetButtonDown("Fire2"))
        {
            dash = true;
        }
    }

    void TempAttackMethodForTesting()
    {
        if (Input.GetButtonDown("Fire1")) animator.SetTrigger("Slice");
    }

    private void Move()
    {
        if (Input.GetButton("Fire2") && Mathf.Abs(rb.velocity.x) > dashHoldSpeedCancel) return;

        if (Mathf.Abs(direction) > 0.01)
        {
            sRenderer.flipX = direction > 0;
            rb.AddForce(new Vector2(direction * speed, 0), ForceMode2D.Impulse);

            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    [SerializeField] float longerDashForce = 1000;
    [SerializeField] float dashHoldSpeedCancel = 10;
    void Dash()
    {
        float dashDirection = System.Convert.ToInt32(sRenderer.flipX) * 2 - 1;

        if (dash)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(dashForce * dashDirection, 0), ForceMode2D.Impulse);
            animator.SetTrigger("Dash");
            dash = false;
        }

        if (Input.GetButton("Fire2") && Mathf.Abs(rb.velocity.x) > dashHoldSpeedCancel)
        {
            rb.AddForce(new Vector2(longerDashForce * dashDirection, 0));
            rb.gravityScale = 0;
            animator.SetBool("Dashing", true);
        }
        else
        {
            rb.gravityScale = 15f;
            animator.SetBool("Dashing", false);
        }
    }

    [SerializeField] float longerJumpForce = 8750;
    [SerializeField] float jumpHoldSpeedCancel = 5f;
    void Jump()
    {
        

        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, validLayers);
        if (isGrounded && rb.velocity.y <= 0) currentJumps = maxJumps;

        if (!isGrounded && Input.GetButton("Jump") && rb.velocity.y > jumpHoldSpeedCancel) rb.AddForce(new Vector2(0, longerJumpForce));

        if (jump)
        {
            animator.SetTrigger("Jump");
            if(rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            currentJumps--;
            jump = false;
        }
    }

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(groundCheckPosition.position, Vector3.back, groundCheckRadius);
    }*/
}
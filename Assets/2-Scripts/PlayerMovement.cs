using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Animator animator;

    [SerializeField] float speed = 1;
    [SerializeField] float jumpForce = 1;
    [SerializeField] float maxJumps = 1;
    [SerializeField] float currentJumps = 1;

    [SerializeField] Transform groundCheckPosition = default;
    [SerializeField] float groundCheckRadius = 0.4f;
    [SerializeField] LayerMask validLayers = default;
    [SerializeField] bool isGrounded = false;

    [SerializeField] SpriteRenderer sRenderer = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody for " + this.name + " not found");
    }
    private void Update()
    {
        Move();
        Jump();
        TempAttackMethodForTesting();
        animator.SetBool("Falling", !isGrounded);
    }

    void TempAttackMethodForTesting()
    {
        if (Input.GetMouseButtonDown(0)) animator.SetTrigger("Slice");
        if (Input.GetMouseButtonDown(1)) animator.SetTrigger("Punch");

        animator.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
    }

    private void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        transform.position += direction * speed * Time.deltaTime;
        
        if (Mathf.Abs(direction.x) > 0.01)
        {
            sRenderer.flipX = direction.x > 0;
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, validLayers);
        if (isGrounded && rb.velocity.y <= 0) currentJumps = maxJumps;

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            animator.SetTrigger("Jump");
            if(rb.velocity.y < 0)
            {
                rb.velocity = Vector3.zero;
            }
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            currentJumps--;
        }
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(groundCheckPosition.position, Vector3.back, groundCheckRadius);
    }
}
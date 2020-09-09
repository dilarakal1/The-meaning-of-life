using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpForce = 1;
    [SerializeField] float maxJumps = 1;
    [SerializeField] float currentJumps = 1;

    [SerializeField] Transform groundCheck = default;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask = default;
    [SerializeField] bool isGrounded = false;

    [SerializeField] SpriteRenderer sRenderer = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody for " + this.name + " not found");
    }
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundMask);
        if (isGrounded) currentJumps = maxJumps;

        var direction = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(direction, rb.velocity.y) * speed;
        sRenderer.flipX = direction > 0;
        if(Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            currentJumps--;
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpForce * -2 * -9.81f * rb.gravityScale));
        }
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(groundCheck.position, Vector3.back, groundDistance);
    }
}
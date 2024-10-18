using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7f; // Force applied for jumping
    private Rigidbody2D rb;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        HandleJump();
    }

    // Handle jumping logic
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // Jump when space is pressed and not already jumping
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply upward force
            isJumping = true;
        }

        // If the player holds space, freeze them in the air
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            rb.velocity = Vector2.zero; // Stop movement (freeze in the air)
        }

        // Allow gravity to bring the ball when space is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    // Detect collision with the ground to reset jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false; // Allow jumping again after landing
        }
    }
}

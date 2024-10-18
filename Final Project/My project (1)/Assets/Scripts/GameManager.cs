using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gravityForce = 9.81f; // Force applied for gravity
    private bool gravityToLeft = true; // Start with gravity pulling left

    void Start()
    {
        SetGravityDirection(); // Set initial gravity direction to left
    }

    void Update()
    {
        HandleGravitySwitch();
    }

    // Switch gravity direction based on space bar press
    void HandleGravitySwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // When space is pressed, toggle gravity
        {
            gravityToLeft = !gravityToLeft; // Switch gravity direction
            SetGravityDirection(); // Apply the new gravity direction
        }
    }

    // Set gravity direction
    void SetGravityDirection()
    {
        if (gravityToLeft)
        {
            Physics2D.gravity = new Vector2(-gravityForce, 0); // Gravity pulls to the left
        }
        else
        {
            Physics2D.gravity = new Vector2(gravityForce, 0); // Gravity pulls to the right
        }
    }
}

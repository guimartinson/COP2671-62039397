using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Gravity and player control variables
    public float gravityForce = 9.81f; // Force applied for gravity
    private bool gravityToLeft = true; // Start with gravity pulling left
    public GameObject player; // Reference to the player object

    // Timer variables
    public float timerDuration = 60f; // Duration in seconds
    private float currentTime;
    private bool isPlayerDead = false;
    public TextMeshProUGUI timerText; // Reference to the UI Text component for displaying the timer

    void Start()
    {
        currentTime = timerDuration; // Initialize the timer
        SetGravityDirection(); // Set initial gravity direction to left
    }

    void Update()
    {
        HandleGravitySwitch();
        CheckPlayerBounds(); // Check if player is out of bounds
        UpdateTimer(); // Update the timer every frame
    }

    // Handle gravity switching based on space bar press
    void HandleGravitySwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlayerDead) // Only switch gravity if the player is alive
        {
            gravityToLeft = !gravityToLeft; // Toggle gravity direction
            SetGravityDirection(); // Apply the new gravity direction
        }
    }

    // Set the direction of gravity
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

    // Check if the player goes out of screen bounds
    void CheckPlayerBounds()
    {
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);

        if (playerViewportPosition.x < 0 || playerViewportPosition.x > 1 || playerViewportPosition.y < 0 || playerViewportPosition.y > 1)
        {
            PlayerDied(); // Player is out of bounds, trigger death
        }
    }

    // Update the timer every frame
    void UpdateTimer()
    {
        if (!isPlayerDead)
        {
            currentTime -= Time.deltaTime; // Decrease time

            // Update the timer text on screen
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0)
            {
                currentTime = 0; // Cap timer at zero
                WinGame(); // Call win function when time reaches zero
            }
        }
    }

    // Call this function to handle player death
    void PlayerDied()
    {
        isPlayerDead = true; // Stop the timer by setting isPlayerDead to true
        Debug.Log("Player died!"); // Optional: Log player death for debugging
    }

    // Call this function to trigger win condition
    void WinGame()
    {
        Debug.Log("You Win!");
        // Optional: Add code here for a win screen or to end the game.
    }
}

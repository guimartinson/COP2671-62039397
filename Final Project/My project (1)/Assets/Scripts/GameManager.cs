using System.Collections;
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

    public ParticleSystem outOfBoundsParticles;

    public Canvas canvas; // Reference to the Canvas object that holds the UI elements

    // UI Buttons
    public GameObject playButton; // Play button reference
    public GameObject restartButton; // Restart button reference

    public GameObject MenuBackground;

    public GameObject TitleText;

    // Game state
    private bool isGameRunning = false; // Track if the game has started

    // AudioManager
    public AudioSource audioManager; // Reference to the AudioManager's AudioSource

    public Button muteButton; // Reference to the mute button
    public TextMeshProUGUI muteButtonText; // Reference to the button's text
    private bool isSoundMuted = false;

    void Start()
{
    currentTime = timerDuration; // Initialize the timer
    SetGravityDirection(); // Set initial gravity direction to left

    // Initial setup: Pause the game and show the Play button
    Time.timeScale = 0; // Freeze game
    playButton.SetActive(true); // Show Play button
    restartButton.SetActive(false); // Hide Restart button
    MenuBackground.SetActive(true); // Show the menu background
    timerText.gameObject.SetActive(false); // Hide timer initially
    audioManager.Stop(); // Ensure the background music is off
    TitleText.SetActive(true); // Hide Title Text

     // Set initial text based on the sound state
        UpdateMuteButtonText();
        
        // Add a listener to the mute button
        muteButton.onClick.AddListener(ToggleSound);

}

// Toggle sound mute/unmute
    public void ToggleSound()
    {
        isSoundMuted = !isSoundMuted; // Toggle the mute state

        // Mute or unmute the audio
        audioManager.mute = isSoundMuted;

        // Update the button text
        UpdateMuteButtonText();
    }

    private void UpdateMuteButtonText()
    {
        if (isSoundMuted)
        {
            muteButtonText.text = "Sound OFF";
        }
        else
        {
            muteButtonText.text = "Sound ON";
        }
    }

    void Update()
    {
        if (!isGameRunning) return; // If the game isn't running, skip updates

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
            timerText.text = "Timer: " + Mathf.Ceil(currentTime).ToString();

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
    isPlayerDead = true; // Mark the player as dead
    isGameRunning = false; // Stop game updates

    // Play particle effects
    if (outOfBoundsParticles)
    {
        ParticleSystem particles = Instantiate(outOfBoundsParticles, player.transform.position, Quaternion.identity);
        particles.Play();
        Destroy(particles.gameObject, particles.main.duration); // Destroy after the effect finishes
    }

    Debug.Log("Player died!"); // Optional: Log player death for debugging

    // Start a coroutine to delay the game freeze and restart button
    StartCoroutine(HandleDeathSequence(3f)); // 3-second delay before freezing and showing restart button
}

IEnumerator HandleDeathSequence(float delay)
{
    yield return new WaitForSeconds(delay); // Wait for the specified delay

    // Freeze the game and show the restart button
    Time.timeScale = 0; // Pause the game
    restartButton.SetActive(true); // Show Restart button
    MenuBackground.SetActive(true);
    
}


    // Call this function to trigger win condition
    void WinGame()
    {
        Time.timeScale = 0; // Pause the game
        isGameRunning = false; // Stop game updates
        restartButton.SetActive(true); // Show Restart button
        MenuBackground.SetActive(true);
        Debug.Log("You Win!");
    }

    // Called when the Play button is pressed
    public void StartGame()
{
    Time.timeScale = 1; // Resume the game
    isGameRunning = true; // Start game updates
    playButton.SetActive(false); // Hide Play button
    TitleText.SetActive(false); // Hide Title Text
    timerText.gameObject.SetActive(true); // Make sure the timer is visible
    audioManager.Play(); // Start background music

    MenuBackground.SetActive(false); // Show the menu background

    StartCoroutine(StartGameAfterDelay(2f)); // Delay game start by 2 seconds
    StartCoroutine(FadeInMusic(2f)); // Fade in music over 2 seconds
}

IEnumerator StartGameAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay); // Wait for the specified delay
    Time.timeScale = 1; // Resume the game
    isGameRunning = true; // Start game updates
}

    // Called when the Restart button is pressed
    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        isGameRunning = true; // Start game updates
        restartButton.SetActive(false); // Hide Restart button
        MenuBackground.SetActive(false);
        isPlayerDead = false; // Reset player status
        currentTime = timerDuration; // Reset the timer
        player.transform.position = Vector3.zero; // Reset player position
        SetGravityDirection(); // Reset gravity direction
        audioManager.Play(); // Restart background music
        Debug.Log("Game Restart!");
    }

    IEnumerator FadeOutMusic(float duration)
{
    float startVolume = audioManager.volume;

    while (audioManager.volume > 0)
    {
        audioManager.volume -= startVolume * Time.deltaTime / duration;
        yield return null;
    }

    audioManager.Stop(); // Stop the audio completely
    audioManager.volume = startVolume; // Reset the volume for future use
}

IEnumerator FadeInMusic(float duration)
{
    audioManager.volume = 0f; // Start at volume 0
    audioManager.Play(); // Start playing the music

    while (audioManager.volume < 0.2f)
    {
        audioManager.volume += Time.deltaTime / duration;
        yield return null;
    }
}

}

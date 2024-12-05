using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
public AudioSource hitSound; // Reference to the AudioSource for the hit sound
public AudioClip hitClip; // Sound clip for when the player hits a column

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Column")) // Ensure the column has the "Column" tag
    {
        // Play the hit sound
        if (hitSound && hitClip)
        {
            hitSound.PlayOneShot(hitClip);
        }

        Debug.Log("Player hit the column!");
    
        }
    }
}

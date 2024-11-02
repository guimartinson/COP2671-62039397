// AudioManager.cs
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        
        audioSource.Play();
    }
}

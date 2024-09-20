using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody PlayerRb;
    public float jumpForce;
    public float gravityModifier;

    public bool isOnGround = true;

    private Animator playerAnim;

    public ParticleSystem explosionParticle;

    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;

    public AudioClip crashSound;

    private AudioSource playerAudio;



    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) {
        PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        dirtParticle.Stop();
        playerAudio.PlayOneShot(jumpSound, 1.0f);
        playerAnim.SetTrigger("Jump_trig");
    }}

    public bool gameOver = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            dirtParticle.Stop();
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
}
}
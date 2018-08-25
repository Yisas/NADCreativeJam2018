﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float moveSpeed;
    public float climbSpeed;
    public float jumpForce;

    public AudioClip scaredAudioClip;

    public float vibrationInterval;
    public float vibrationIntensity;

    // States
    private bool isJumpLockedWhenNearPitfall = false;
    public bool isClimbLocked = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isInsidePitfallAproachZone = false;            // Is inside an area where the player is scared of jumping
    public bool isFacingClimbable = false;                     // When facing climbable, movement logic will change
    public bool isSqueezing = false;                    // When facing squeezable, movement logic will change

    // Private attributes
    private float vibrationTimer = 0;

    // References
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float initGravityScale;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initGravityScale = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        isGrounded = Mathf.Abs(rb.velocity.y) == 0;

        if (isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumping = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSqueezing)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, 0.5f, 0.5f), transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }

        // Reset vibration
        if (vibrationTimer > 0)
        {
            vibrationTimer -= Time.deltaTime;
            if (vibrationTimer <= 0)
            {
                GamePad.SetVibration(0, 0, 0);
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool playerIsMovingCharacter = Mathf.Abs(horizontalInput) != 0;
        
        if (playerIsMovingCharacter)
        {
            // If not facing climbable, or moving left, move normally...
            if (!isFacingClimbable)
                transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
            // ... Else climb if moving right
            else
            {
                // ... also check for memory effect
                if (!isClimbLocked)
                {
                    //Vector2 currVel = GetComponent<Rigidbody2D>().velocity;
                    //GetComponent<Rigidbody2D>().velocity = new Vector2(currVel.x, climbSpeed);
                    Vector3 vectorControl = new Vector3(Time.deltaTime * climbSpeed * horizontalInput * 0.1f, Mathf.Abs(Time.deltaTime * climbSpeed * horizontalInput), 0);
                    transform.Translate(vectorControl, 0);
                }
                else
                {
                    ScaredFeedback();
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (CheckIfCanJump())
            {
                isJumping = true;
                isGrounded = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PitfallAproach")
        {
            isInsidePitfallAproachZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "PitfallAproach")
        {
            isInsidePitfallAproachZone = false;
        }
    }

    public void LockUnlockJumpWhenNearPitfall(bool lockJump)
    {
        isJumpLockedWhenNearPitfall = lockJump;
    }

    public void LockUnlockClimbing(bool lockClimb)
    {
        isClimbLocked = lockClimb;
    }

    private bool CheckIfCanJump()
    {
        // Check if grounded
        if (!isGrounded || isJumping || isFacingClimbable)
        {
            return false;
        }
        else
        {
            // check for memory effect
            if (isJumpLockedWhenNearPitfall)
            {
                if (isInsidePitfallAproachZone)
                {
                    ScaredFeedback();
                    return false;
                }
            }
        }

        return true;
    }

    public void SetIsFacingClimbable(bool value)
    {
        isFacingClimbable = value;

        if (value)
            rb.gravityScale = 0;
        else
            rb.gravityScale = initGravityScale;
    }

    public void SetIsSqueezing(bool value)
    {
        isSqueezing = value;
    }

    /// <summary>
    /// Sound + vibration
    /// </summary>
    private void ScaredFeedback()
    {
        GamePad.SetVibration(0, vibrationIntensity, vibrationIntensity);
        vibrationTimer = vibrationInterval;

        if (!audioSource.isPlaying)
            // Play "scared of pitfall" sound
            audioSource.PlayOneShot(scaredAudioClip, 1);
    }

    public void Die()
    {
        rb.velocity = new Vector2(0, 0);
        PlayerMemoryController.Instance.FlushAllMemories();
        GManager.Instance.RespawnPlayer();
    }
}
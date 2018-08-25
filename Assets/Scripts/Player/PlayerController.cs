﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    public float moveSpeed;
    public float climbSpeed;
    public float jumpForce;

    // States
    private bool isJumpLockedWhenNearPitfall = false;
    private bool isClimbLocked = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isInsidePitfallAproachZone = false;            // Is inside an area where the player is scared of jumping
    private bool isFacingClimbable = false;                     // When facing climbable, movement logic will change

    // References
    private Rigidbody2D rb;
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
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool playerIsMovingCharacter = Mathf.Abs(horizontalInput) > 0;

        if (playerIsMovingCharacter)
        {
            // If not facing climbable, move normally...
            if (!isFacingClimbable)
                transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
            // ... Else climb if moving right
            else
            {
                // ... also check for memory effect
                if (!isClimbLocked)
                {
                    if (horizontalInput >= 0)
                    {
                        transform.Translate(new Vector3(0, Time.deltaTime * climbSpeed * horizontalInput, 0));
                    }
                }
                else
                {
                    // TODO: Don't want to climb feedback goes here
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

    private void FixedUpdate()
    {
        isGrounded = Mathf.Abs(rb.velocity.y) == 0;


        if (isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumping = false;
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
        if (!isGrounded || isJumping)
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
}
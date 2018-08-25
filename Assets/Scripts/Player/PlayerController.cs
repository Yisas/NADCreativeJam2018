using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    public float moveSpeed;
    public float jumpForce;

    // States
    private bool isJumpLockedWhenNearPitfall = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isInsidePitfallAproachZone = false;            // Is inside an area where the player is scared of jumping

    // References
    private Rigidbody2D rb;

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
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        bool playerIsMovingCharacter = Mathf.Abs(horizontalInput) > 0;

        if (playerIsMovingCharacter)
        {
            transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
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
}
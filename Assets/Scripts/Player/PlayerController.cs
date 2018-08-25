using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    public float moveSpeed;
    public float jumpForce;

    // States
    private bool isJumpLocked = false;
    private bool isJumping = false;
    private bool isGrounded = false;

    // References
    private Rigidbody2D rb;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {

        float horizontalInput = Input.GetAxis("Horizontal");
        bool playerIsMovingCharacter = Mathf.Abs(horizontalInput) > 0;

        if (playerIsMovingCharacter)
        {
            transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isJumpLocked)
        {
            isJumping = true;
            isGrounded = false;
        }
	}

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    public void LockUnlockJump(bool lockJump)
    {
        isJumpLocked = lockJump;
    }
}
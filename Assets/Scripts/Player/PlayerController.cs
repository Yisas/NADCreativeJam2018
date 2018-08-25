using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;

    private bool isJumping = false;
    private Rigidbody2D rb;
    
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

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
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
}
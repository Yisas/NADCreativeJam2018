using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float horizontalInput = Input.GetAxis("Horizontal");
        bool playerIsMovingCharacter = Mathf.Abs(horizontalInput) > 0;

        if (playerIsMovingCharacter)
        {
            transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
        }
	}

    private void FixedUpdate()
    {
        
    }
}
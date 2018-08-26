using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Climbable")
        {
            playerController.SetIsFacingClimbable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Climbable")
        {
            playerController.SetIsFacingClimbable(false);
        }
    }
}

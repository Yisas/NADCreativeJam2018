using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Climbable")
        {
            PlayerController.Instance.SetIsFacingClimbable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Climbable")
        {
            PlayerController.Instance.SetIsFacingClimbable(false);
        }
    }
}

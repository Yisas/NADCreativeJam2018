using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeCheck : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != PlayerController.Instance.gameObject) return;

        if (collision.transform.tag == "Squeezable")
        {
            PlayerController.Instance.SetIsFacingClimbable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != PlayerController.Instance.gameObject) return;

        if (collision.transform.tag == "Squeezable")
        {
            PlayerController.Instance.SetIsFacingClimbable(false);
        }
    }
}

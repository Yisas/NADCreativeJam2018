using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeCheck : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Squeezable")
        {
            PlayerController.Instance.SetIsSqueezing(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Squeezable")
        {
            PlayerController.Instance.SetIsSqueezing(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMemoryController>().ComputeScore();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GManager>().NextScene();
        }
    }
}

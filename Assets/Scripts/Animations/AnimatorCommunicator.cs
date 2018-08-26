using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCommunicator : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void TriggerJump()
    {
        playerController.TriggerJump();
    }
}

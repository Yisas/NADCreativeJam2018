using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCommunicator : MonoBehaviour
{
    public AudioClip stepSound;

    private AudioSource audioSource;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerJump()
    {
        playerController.TriggerJump();
    }

    public void Step()
    {
        audioSource.PlayOneShot(stepSound);
    }
}

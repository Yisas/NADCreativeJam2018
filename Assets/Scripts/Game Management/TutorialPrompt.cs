using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    public string tutorialMessageToDisplay;
    private BuddyAIController buddyAI;

    // Use this for initialization
    void Start()
    {
        buddyAI = GameObject.FindGameObjectWithTag("BuddyAI").GetComponent<BuddyAIController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            buddyAI.DisplayMessage(tutorialMessageToDisplay);
        }
    }

}

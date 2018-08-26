using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyAIController : MonoBehaviour
{
    public Transform target;
    public Transform playerAnchor;
    private float damping = 2.5f;

    public string[] memoryPickupSoundbites;
    public float timeBetweenTutorialMessages;

    private int lastSelectedSoundbite = 0;
    private float tutorialSequenceTimer = 0;

    // References
    private PlayerMemoryController playerMemoryController;
    private BuddyAICanvas canvasController;
    private Animator anim;
    private bool displayingFlushTutorial = false;
    private int flushTutorialSequenceCount = 0;

    private void Start()
    {
        playerMemoryController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMemoryController>();
        canvasController = GetComponentInChildren<BuddyAICanvas>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (displayingFlushTutorial)
        {
            tutorialSequenceTimer -= Time.deltaTime;

            if (tutorialSequenceTimer <= 0)
            {
                switch (flushTutorialSequenceCount)
                {
                    case 0:
                        canvasController.DisplayMessage("Sometimes you get bad memories");
                        flushTutorialSequenceCount++;
                        tutorialSequenceTimer = timeBetweenTutorialMessages;
                        break;
                    case 1:
                        canvasController.DisplayMessage("It's not your fault!");
                        flushTutorialSequenceCount++;
                        tutorialSequenceTimer = timeBetweenTutorialMessages;
                        break;
                    case 2:
                        canvasController.DisplayMessage("Press X to forget");
                        flushTutorialSequenceCount++;
                        tutorialSequenceTimer = timeBetweenTutorialMessages;
                        break;
                    case 3:
                        displayingFlushTutorial = false;
                        tutorialSequenceTimer = 0;
                        break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!displayingFlushTutorial)
        {
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, target.position.x, Time.deltaTime * damping),
                                                   Mathf.Lerp(this.transform.position.y, target.position.y, Time.deltaTime * damping), 0);

            if (target != playerAnchor)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                // Start opening lid on aproach
                if (distance < 3.0f)
                {
                    anim.SetTrigger("openLid");
                }

                if (distance < 1.5f)
                {
                    Destroy(target.parent.gameObject);
                    target = playerAnchor;
                    playerMemoryController.AssignNewMemory();

                    DisplayMessage(ChooseRandomSoundbite());
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (target == playerAnchor) //if on player anchor
        {
            target = other.transform;
        }
        else
        {
            if (!target)
            {
                target = playerAnchor;
            }
        }
    }

    public void DisplayMessage(string message)
    {
        if (!displayingFlushTutorial)
            canvasController.DisplayMessage(message);
    }

    private string ChooseRandomSoundbite()
    {
        int i;

        do
        {
            i = Random.Range(0, memoryPickupSoundbites.Length);
        }
        while (i == lastSelectedSoundbite);

        lastSelectedSoundbite = i;

        return memoryPickupSoundbites[i];
    }

    public void TriggerFlushTutorial()
    {
        displayingFlushTutorial = true;
        tutorialSequenceTimer = timeBetweenTutorialMessages;

        canvasController.DisplayMessage("Sometimes you get bad memories");
        flushTutorialSequenceCount++;
        tutorialSequenceTimer = timeBetweenTutorialMessages;
    }
}

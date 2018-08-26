using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyAIController : MonoBehaviour
{
    public Transform target;
    public Transform playerAnchor;
    private float damping = 2.5f;

    // References
    private PlayerMemoryController playerMemoryController;
    private BuddyAICanvas canvasController;
    private Animator anim;

    private void Start()
    {
        playerMemoryController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMemoryController>();
        canvasController = GetComponentInChildren<BuddyAICanvas>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Test Button"))
        {
            canvasController.DisplayMessage("Testing :)");
        }
    }

    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, target.position.x, Time.deltaTime * damping),
                                               Mathf.Lerp(this.transform.position.y, target.position.y, Time.deltaTime * damping), 0);

        if (target != playerAnchor)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            // Start opening lid on aproach
            if(distance < 3.0f)
            {
                anim.SetTrigger("openLid");
            }

            if (distance < 1.5f)
            {
                Destroy(target.parent.gameObject);
                target = playerAnchor;
                playerMemoryController.AssignNewMemory();

                canvasController.DisplayMessage("New Memory!");
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
}

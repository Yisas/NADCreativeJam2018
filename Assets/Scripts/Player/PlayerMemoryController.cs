using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryController : MonoBehaviour
{
    public enum MemoryTypes { None, Pitfall, Climb, PitfallBoost, ClimbBoost };
    public static readonly int maxNumberOfMemories = 3;       // Mamimum amount of memories allowed
    private readonly int numOfMemoryTypes = 4;                // Number of memory types, disregarding "None"

    public bool randomMemoryAssignmentLoopOn = false;
    public float minMemoryChangeTimer;
    public float maxMemoryChangeTimer;
    private float memoryChangeTimer = 0;

    public Transform bouncingMemorySpawnPosition;
    public GameObject bouncingMemory;
    public float flushVibrationIntensity;
    public float flushVibrationDuration;
    public AudioClip vomitAudioClip;

    private Queue<MemoryTypes> memories = new Queue<MemoryTypes>();

    // References
    private PlayerController playerController;
    private MemoryCanvasController memoryCanvasController;
    private Animator anim;
    private AudioSource audioSource;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        memoryCanvasController = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MemoryCanvasController>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cycle Memory"))
        {
            CycleMemory();
            return;
        }

        if (randomMemoryAssignmentLoopOn)
        {
            memoryChangeTimer -= Time.deltaTime;

            if (memoryChangeTimer <= 0)
            {
                AssignNewMemory();

                // get a new countdown to the next change
                memoryChangeTimer = Random.Range(minMemoryChangeTimer, maxMemoryChangeTimer);
            }
        }
    }

    public void AssignNewMemory()
    {
        // Find an elegible memory to assign (not already in the memory pool)
        MemoryTypes memoryToAssign;
        do
        {
            // Choose a new memory to asign
            memoryToAssign = (MemoryTypes)Random.Range(1, numOfMemoryTypes + 1);   // +1 since random funct is exclusive
        }
        while (memories.Contains(memoryToAssign));

        // If the memory stack is not full, occupy an empty one...
        if (memories.Count < maxNumberOfMemories)
        {
            if (memoryToAssign != MemoryTypes.None)
            {
                ApplyMemory(memoryToAssign, false);
            }

            return;
        }

        ApplyMemory(memoryToAssign);
    }

    private void RemoveMemory()
    {
        if (memories.Count == 0)
            return;

        MemoryTypes priorMemory = memories.Dequeue();
        if (priorMemory == MemoryTypes.None)
        {
            return;
        }
        else
        {
            switch (priorMemory)
            {
                case MemoryTypes.Pitfall:
                    ApplyPitfallMemory(false);
                    break;
                case MemoryTypes.PitfallBoost:
                    ApplyPitfallMemory(true);
                    break;
                case MemoryTypes.Climb:
                    ApplyClimbMemory(false);
                    break;
                case MemoryTypes.ClimbBoost:
                    ApplyClimbMemory(true);
                    break;
            }
        }
    }

    /// <summary>
    /// Removes the prior one by default
    /// </summary>
    /// <param name="memoryToAssign"></param>
    /// <param name="remove"></param>
    private void ApplyMemory(MemoryTypes memoryToAssign, bool remove = true)
    {
        // Don't apply same type twice
        if (!memories.Contains(memoryToAssign))
        {
            Debug.Log("Applying memory " + memoryToAssign);

            // Remove prior memory
            if (remove)
                RemoveMemory();

            switch (memoryToAssign)
            {
                case MemoryTypes.Pitfall:
                    ApplyPitfallMemory(true);
                    break;
                case MemoryTypes.PitfallBoost:
                    ApplyPitfallMemory(false);
                    break;
                case MemoryTypes.Climb:
                    ApplyClimbMemory(true);
                    break;
                case MemoryTypes.ClimbBoost:
                    ApplyClimbMemory(false);
                    break;
            }

            // Update list
            memories.Enqueue(memoryToAssign);

            // Display memory effect in UI
            memoryCanvasController.ChangeMemory(memoryToAssign);
        }
    }

    private void ApplyPitfallMemory(bool active)
    {
        playerController.LockUnlockJumpWhenNearPitfall(active);
    }

    private void ApplyClimbMemory(bool active)
    {
        playerController.LockUnlockClimbing(active);
    }

    /// <summary>
    /// Remove them ALL
    /// </summary>
    public void FlushAllMemories()
    {
        int prevNumOfMemories = memories.Count;

        if (prevNumOfMemories > 0)
        {
            anim.SetTrigger("vomit");
            audioSource.PlayOneShot(vomitAudioClip);

            for (int i = 0; i < prevNumOfMemories; i++)
            {
                GameObject go = GameObject.Instantiate(bouncingMemory, bouncingMemorySpawnPosition.position, bouncingMemory.transform.rotation);
            }

            // Rumble
            playerController.Rumble(flushVibrationIntensity, flushVibrationDuration);
        }

        for (int i = 0; i < prevNumOfMemories; i++)
        {
            RemoveMemory();
        }

        ApplyPitfallMemory(false);
        ApplyClimbMemory(false);
        memoryCanvasController.FlushMemories();
    }

    /// <summary>
    /// User cycles a memory in their pool
    /// </summary>
    private void CycleMemory()
    {
        FlushAllMemories();
    }

    public void ComputeScore()
    {
        int scoreCount = 0;

        foreach (MemoryTypes m in memories)
        {
            if (m == MemoryTypes.ClimbBoost || m == MemoryTypes.PitfallBoost)
            {
                scoreCount++;
            }
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GManager>().IncreaseScore(scoreCount);
    }
}

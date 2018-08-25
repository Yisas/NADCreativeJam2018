using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryController : MonoBehaviour
{
    public static PlayerMemoryController Instance;

    public enum MemoryTypes { None, Pitfall, Climb };
    public static readonly int maxNumberOfMemories = 3;       // Mamimum amount of memories allowed
    private readonly int numOfMemoryTypes = 2;          // Number of memory types, disregarding "None"

    public float minMemoryChangeTimer;
    public float maxMemoryChangeTimer;
    private float memoryChangeTimer = 0;

    private Queue<MemoryTypes> memories = new Queue<MemoryTypes>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cycle Memory"))
        {
            CycleMemory();
            return;
        }

        memoryChangeTimer -= Time.deltaTime;

        if (memoryChangeTimer <= 0)
        {
            AssignNewMemory();

            // get a new countdown to the next change
            memoryChangeTimer = Random.Range(minMemoryChangeTimer, maxMemoryChangeTimer);
        }
    }

    void AssignNewMemory()
    {
        // Choose a new memory to asign
        MemoryTypes memoryToAssign = (MemoryTypes)Random.Range(0, numOfMemoryTypes + 1);   // +1 since random funct is exclusive

        // If the memory stack is not full, occupy an empty one...
        if (memories.Count < maxNumberOfMemories)
        {
            if (memoryToAssign != MemoryTypes.None)
            {
                ApplyMemory(memoryToAssign, false);
            }

            return;
        }

        // Apply memory effect
        // Remove if none ...
        if (memoryToAssign == MemoryTypes.None)
        {
            RemoveMemory();
        }
        // ... else apply effect
        else
        {
            ApplyMemory(memoryToAssign);
        }
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
                case MemoryTypes.Climb:
                    ApplyClimbMemory(false);
                    break;
            }

            // Display memory effect in UI
            MemoryCanvasController.instance.ChangeMemory(MemoryTypes.None);
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
                case MemoryTypes.Climb:
                    ApplyClimbMemory(true);
                    break;
            }

            // Update list
            memories.Enqueue(memoryToAssign);

            // Display memory effect in UI
            MemoryCanvasController.instance.ChangeMemory(memoryToAssign);
        }
    }

    private void ApplyPitfallMemory(bool active)
    {
        PlayerController.Instance.LockUnlockJumpWhenNearPitfall(active);
    }

    private void ApplyClimbMemory(bool active)
    {
        PlayerController.Instance.LockUnlockClimbing(active);
    }

    /// <summary>
    /// Remove them ALL
    /// </summary>
    public void FlushAllMemories()
    {
        for (int i = 0; i < memories.Count; i++)
        {
            RemoveMemory();
        }
    }

    /// <summary>
    /// User cycles a memory in their pool
    /// </summary>
    private void CycleMemory()
    {
        RemoveMemory();
    }
}

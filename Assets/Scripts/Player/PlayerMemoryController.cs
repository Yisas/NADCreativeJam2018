using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryController : MonoBehaviour {

    public enum MemoryTypes { None, Pitfall, Climb};
    private readonly int maxNumberOfMemories = 3;       // Mamimum amount of memories allowed
    private readonly int numOfMemoryTypes = 2;          // Number of memory types, disregarding "None"

    public float minMemoryChangeTimer;
    public float maxMemoryChangeTimer;
    private float memoryChangeTimer = 0;

    // How many memories you have
    private int memoryCount = 0;

    private List<MemoryTypes> memories = new List<MemoryTypes>();

	// Use this for initialization
	void Start () {

        // Fill the memory list
        for (int i = 0; i < maxNumberOfMemories; i++)
        {
            memories.Add(MemoryTypes.None);
        }
    }
	
	// Update is called once per frame
	void Update () {

        memoryChangeTimer -= Time.deltaTime;

		if(memoryChangeTimer <= 0)
        {
            AssignNewMemory(MemoryTypes.Pitfall);

            // get a new countdown to the next change
            memoryChangeTimer = Random.Range(minMemoryChangeTimer, maxMemoryChangeTimer);
        }
	}

    void AssignNewMemory(MemoryTypes memory)
    {
        // Choose a new memory to asign
        MemoryTypes memoryToAssign = (MemoryTypes) Random.Range(0, numOfMemoryTypes + 1);   // +1 since random funct is exclusive

        // Choose a memory to change
        int memoryIndexToChange = 0;
        // If the memory stack is not full, occupy an empty one...
        if (memoryCount < maxNumberOfMemories)
        {
            if (memoryToAssign != MemoryTypes.None)
            {
                memoryIndexToChange = memoryCount;
                memoryCount++;
                MemoryCanvasController.instance.ChangeMemory(memoryIndexToChange, memoryToAssign);
                memories[memoryIndexToChange] = memoryToAssign;
                return;
            }
            else
                // ... nones are ignored until stack is filled
                return;
        }
        // ...  else change a random one
        else
        {
            memoryIndexToChange = Random.Range(0, 2);
        }

        // Apply memory effect
        // Remove if none ...
        if (memoryToAssign == MemoryTypes.None)
        {
            RemoveMemoryAtIndex(memoryIndexToChange);
            memories[memoryIndexToChange] = MemoryTypes.None;
        }
        // ... else apply effect
        else
        {
            // Update list
            memories[memoryIndexToChange] = memoryToAssign;

            // Don't apply same type twice
            if (!memories.Contains(memoryToAssign))
            {
                Debug.Log("Applying memory " + memoryToAssign + " at index " + memoryIndexToChange);

                switch (memoryToAssign)
                {
                    case MemoryTypes.Pitfall:
                        ApplyPitfallMemory(true);
                        break;
                }
            }
        }

        // Display memory effect in UI
        MemoryCanvasController.instance.ChangeMemory(memoryIndexToChange, memoryToAssign);
    }

    private void RemoveMemoryAtIndex(int index)
    {
        MemoryTypes priorMemory = memories[index];

        if(priorMemory == MemoryTypes.None)
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
            }
        }
    }

    private void ApplyPitfallMemory(bool active)
    {
        PlayerController.Instance.LockUnlockJump(active);
    }
}

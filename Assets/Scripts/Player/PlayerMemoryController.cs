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

    private MemoryTypes[] memories = new MemoryTypes[3];

	// Use this for initialization
	void Start () {
		
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
        if (memoryCount < maxNumberOfMemories && memoryToAssign != MemoryTypes.None)
        {
            memoryIndexToChange = memoryCount;
            memoryCount++;
        }
        // ...  else change a random one
        else
        {
            memoryIndexToChange = Random.Range(0, 2);
        }

        MemoryCanvasController.instance.ChangeMemory(memoryIndexToChange, memoryToAssign);
    }
}

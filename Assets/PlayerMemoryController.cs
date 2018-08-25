using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryController : MonoBehaviour {

    public enum MemoryTypes { None, Pitfall};

    public float minMemoryChangeTimer;
    public float maxMemoryChangeTimer;
    private float memoryChangeTimer = 0;

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
        
    }
}

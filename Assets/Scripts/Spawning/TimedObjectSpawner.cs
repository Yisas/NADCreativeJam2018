using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectSpawner : MonoBehaviour
{
    public float timeToRespawn;
    public GameObject objectToSpawn;

    private float respawnTimer = 0;
    private bool isSpawned = true;

    private void Start()
    {
        Spawn();   
    }

    private void Update()
    {
        if (!isSpawned)
        {
            respawnTimer -= Time.deltaTime;

            if(respawnTimer <= 0)
            {
                Spawn();
            }
        }
    }

    public void StartSpawningNewObject()
    {
        respawnTimer = timeToRespawn;

        isSpawned = false;
    }

    private void Spawn()
    {
        GameObject go = Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);

        // Attach a listener
        TimedObjectSpawnerListener listener = go.AddComponent<TimedObjectSpawnerListener>();
        listener.spawner = this;

        isSpawned = true;
    }
}

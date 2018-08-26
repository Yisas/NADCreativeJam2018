using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectSpawnerListener : MonoBehaviour {

    public TimedObjectSpawner spawner;

    private void OnDestroy()
    {
        spawner.StartSpawningNewObject();
    }
}

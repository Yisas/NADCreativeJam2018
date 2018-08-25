using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour {

    public static GManager Instance;

    public PlayerController player;
    public Transform spawnPoint;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = spawnPoint.transform.position;
    }
}

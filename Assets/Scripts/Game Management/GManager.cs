using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager Instance;

    public PlayerController player;
    public Transform spawnPoint;

    private static int numberOfScenes = 2;
    private static int currentSceneBuildIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Test Button"))
        {
            NextScene();
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = spawnPoint.transform.position;
    }

    public void NextScene()
    {
        Debug.Log(currentSceneBuildIndex);
        currentSceneBuildIndex++;

        if (currentSceneBuildIndex > numberOfScenes - 1)
        {
            // Reset counter when you reach the end
            currentSceneBuildIndex = 0;
            SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(currentSceneBuildIndex);
    }
}

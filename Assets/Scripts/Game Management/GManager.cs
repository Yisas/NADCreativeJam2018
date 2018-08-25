using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager Instance;

    public PlayerController player;
    public Transform spawnPoint;
    public float fadeOutDelayTime;

    private static int numberOfScenes = 2;
    private static int currentSceneBuildIndex = 0;
    private float fadeOutTimer = 0;
    private bool sceneTransitionTriggered = false;

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

        if (sceneTransitionTriggered)
        {
            fadeOutTimer -= Time.deltaTime;

            if (fadeOutTimer <= 0)
            {
                LoadNextScene();
                sceneTransitionTriggered = false;
                fadeOutTimer = 0;
            }
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = spawnPoint.transform.position;
    }

    public void NextScene()
    {
        fadeOutTimer = fadeOutDelayTime;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().FadeOut();
        sceneTransitionTriggered = true;
    }

    private void LoadNextScene()
    {
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

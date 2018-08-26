using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public PlayerController player;
    public Transform spawnPoint;
    public float fadeOutDelayTime;

    private int score = 0;
    private static int currentSceneBuildIndex = 0;
    private float fadeOutTimer = 0;
    private bool sceneTransitionTriggered = false;

    private void Update()
    {
        if (Input.GetButtonDown("Next Level"))
        {
            NextScene();
        }

        if (Input.GetButtonDown("Test Button"))
        {
            IncreaseScore();
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

    public void IncreaseScore(int amount = 1)
    {
        score += amount;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().IncreaseScore(amount);
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

        if (currentSceneBuildIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            // Reset counter when you reach the end
            currentSceneBuildIndex = 0;
            SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(currentSceneBuildIndex);
    }
}

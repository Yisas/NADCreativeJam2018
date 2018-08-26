using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public PlayerController player;
    public Transform spawnPoint;
    public float fadeOutDelayTime;
    public float fadeOutDelatTimeFast;

    private static int score = 0;
    private static int currentSceneBuildIndex = 0;
    private float fadeOutTimer = 0;
    private bool fading = false;
    private bool sceneTransitionTriggered = false;
    private bool isInMainMenu = false;

    private void Start()
    {
        isInMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        if (!isInMainMenu)
            SetScore(score);
    }

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

        if (fading)
        {
            fadeOutTimer -= Time.deltaTime;

            if (fadeOutTimer <= 0)
            {
                if (sceneTransitionTriggered)
                {
                    LoadNextScene();
                    sceneTransitionTriggered = false;
                }
                else
                {
                    // Respawning
                    player.transform.position = spawnPoint.transform.position;
                    GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().FadeIn(true);
                }

                fadeOutTimer = 0;
                fading = false;
            }
        }
    }

    public void RespawnPlayer()
    {
        fading = true;
        fadeOutTimer = fadeOutDelatTimeFast;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().FadeOut(true);
    }

    public void IncreaseScore(int amount = 1)
    {
        score += amount;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().IncreaseScore(amount);
    }

    public void SetScore(int amount)
    {
        score = amount;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().SetScore(amount);
    }

    public void NextScene()
    {
        fadeOutTimer = fadeOutDelayTime;
        GameObject.FindGameObjectWithTag("UICanvas").GetComponent<MainUIController>().FadeOut();
        fading = true;
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

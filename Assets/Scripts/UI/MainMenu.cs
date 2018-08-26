using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GManager>().NextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

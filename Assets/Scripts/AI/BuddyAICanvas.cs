using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuddyAICanvas : MonoBehaviour
{
    public GameObject panel;
    private float delayTime = 4.0f;
    private IEnumerator coroutine;

    public void DisplayMessage(string text)
    {
        panel.SetActive(true);
        GetComponentInChildren<Text>().text = text;

        StopAllCoroutines();
        coroutine = dissapear(delayTime);
        StartCoroutine(coroutine);
    }

    // every 2 seconds perform the print()
    private IEnumerator dissapear(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            panel.SetActive(false);
        }
    }
}

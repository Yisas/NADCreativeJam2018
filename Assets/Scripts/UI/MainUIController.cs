using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Image blackSprite;
    public float fadeSpeed = 0.05f;
    public float fadeSpeedFast = 0.1f;

    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        FadeIn();
    }

    public void FadeIn(bool fast = false)
    {
        if (!fast)
        {
            StartCoroutine("FadeInCoroutine");
        }
        else
        {
            StartCoroutine("FadeInCoroutineFast");
        }
    }

    public void FadeOut(bool fast = false)
    {
        if (!fast)
        {
            StartCoroutine("FadeOutCoroutine");
        }
        else
        {
            StartCoroutine("FadeOutCoroutineFast");
        }
    }

    public void IncreaseScore(int amount = 1)
    {
        scoreText.text = (int.Parse(scoreText.text) + amount).ToString();
    }

    public void SetScore(int amount)
    {
        scoreText.text = amount.ToString();
    }

    IEnumerator FadeInCoroutine()
    {
        for (float f = 1f; f >= 0; f -= fadeSpeed)
        {
            Color c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        for (float f = 0f; f <= 1.0f; f += fadeSpeed)
        {
            Color c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }
    }

    IEnumerator FadeInCoroutineFast()
    {
        for (float f = 1f; f >= 0; f -= fadeSpeedFast)
        {
            Color c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOutCoroutineFast()
    {
        for (float f = 0f; f <= 1.0f; f += fadeSpeedFast)
        {
            Color c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }
    }
}

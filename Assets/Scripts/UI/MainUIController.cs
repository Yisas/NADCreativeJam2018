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
        Color c;

        for (float f = 1f; f >= 0; f -= fadeSpeed)
        {
            c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }

        c = blackSprite.color;
        c.a = 0;
        blackSprite.color = c;
    }

    IEnumerator FadeOutCoroutine()
    {
        Color c;

        for (float f = 0f; f <= 1.0f; f += fadeSpeed)
        {
            c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }

        c = blackSprite.color;
        c.a = 1;
        blackSprite.color = c;
    }

    IEnumerator FadeInCoroutineFast()
    {
        Color c;

        for (float f = 1f; f >= 0; f -= fadeSpeedFast)
        {
            c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }

        c = blackSprite.color;
        c.a = 0;
        blackSprite.color = c;
    }

    IEnumerator FadeOutCoroutineFast()
    {
        Color c;

        for (float f = 0f; f <= 1.0f; f += fadeSpeedFast)
        {
            c = blackSprite.color;
            c.a = f;
            blackSprite.color = c;
            yield return null;
        }

        c = blackSprite.color;
        c.a = 1.0f;
        blackSprite.color = c;
    }
}

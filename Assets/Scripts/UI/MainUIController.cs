using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    public SpriteRenderer blackSprite;
    public float fadeSpeed = 0.05f;

	// Use this for initialization
	void Start () {
        FadeIn();
	}

    public void FadeIn()
    {
        StartCoroutine("FadeInCoroutine");
    }

    public void FadeOut()
    {
        StartCoroutine("FadeOutCoroutine");
    }

    IEnumerator FadeInCoroutine()
    {
        for (float f = 1f; f >= 0; f -= fadeSpeed)
        {
            Color c = blackSprite.material.color;
            c.a = f;
            blackSprite.material.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        for (float f = 0f; f <= 1.0f; f += fadeSpeed)
        {
            Color c = blackSprite.material.color;
            c.a = f;
            blackSprite.material.color = c;
            yield return null;
        }
    }
}

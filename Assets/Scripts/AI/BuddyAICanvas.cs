using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuddyAICanvas : MonoBehaviour
{
    private Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayMessage(string text)
    {
        this.text.enabled = true;
        this.text.text = text;
    }
}

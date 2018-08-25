using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCanvasController : MonoBehaviour {

    public static MemoryCanvasController instance;

    public Image[] memorySprites;
    public Sprite pitfallMemorySprite;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="memoryIndexToChange">Starts with 0</param>
    public void ChangeMemory(int memoryIndexToChange)
    {
        memorySprites[memoryIndexToChange].sprite = pitfallMemorySprite;
    }
}

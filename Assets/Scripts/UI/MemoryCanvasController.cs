using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCanvasController : MonoBehaviour
{
    public static MemoryCanvasController instance;

    public Image[] memorySprites;
    public Sprite pitfallMemorySprite;
    public Sprite climbingMemorySprite;

    private int memoryCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="memoryIndexToChange">Starts with 0</param>
    private void ChangeMemoryAtIndex(int memoryIndexToChange, PlayerMemoryController.MemoryTypes memoryType)
    {
        switch (memoryType)
        {
            case PlayerMemoryController.MemoryTypes.None:
                memorySprites[memoryIndexToChange].sprite = null;
                break;

            case PlayerMemoryController.MemoryTypes.Pitfall:
                memorySprites[memoryIndexToChange].sprite = pitfallMemorySprite;
                break;
            case PlayerMemoryController.MemoryTypes.Climb:
                memorySprites[memoryIndexToChange].sprite = climbingMemorySprite;
                break;
        }
    }

    public void ChangeMemory(PlayerMemoryController.MemoryTypes memoryType)
    {
        if(memoryCount < PlayerMemoryController.maxNumberOfMemories)
        {
            ChangeMemoryAtIndex(memoryCount, memoryType);
            memoryCount++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCanvasController : MonoBehaviour
{
    public static MemoryCanvasController instance;

    public Image[] memorySprites;
    public Sprite pitfallMemorySprite;
    public Sprite pitfallMemorySpriteAlternate;
    public Sprite climbingMemorySprite;
    public Sprite climbingMemorySpriteAlternate;

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

            case PlayerMemoryController.MemoryTypes.PitfallBoost:
                memorySprites[memoryIndexToChange].sprite = pitfallMemorySpriteAlternate;
                break;

            case PlayerMemoryController.MemoryTypes.Climb:
                memorySprites[memoryIndexToChange].sprite = climbingMemorySprite;
                break;

            case PlayerMemoryController.MemoryTypes.ClimbBoost:
                memorySprites[memoryIndexToChange].sprite = climbingMemorySpriteAlternate;
                break;
        }
    }

    private void ChangeMemoryAtIndex(int memoryIndexToChange, Sprite spriteToApply)
    {
        memorySprites[memoryIndexToChange].sprite = spriteToApply;
    }

    public void ChangeMemory(PlayerMemoryController.MemoryTypes memoryType)
    {
        if (memoryCount < PlayerMemoryController.maxNumberOfMemories)
        {
            ChangeMemoryAtIndex(memoryCount, memoryType);
            memoryCount++;
        }
        else
        {
            RemoveMemory();
            ChangeMemoryAtIndex(PlayerMemoryController.maxNumberOfMemories - 1, memoryType);
        }
    }

    public void RemoveMemory()
    {
        int i = 0;
        foreach (Image im in memorySprites)
        {
            if (i < PlayerMemoryController.maxNumberOfMemories - 1)
            {
                ChangeMemoryAtIndex(i, memorySprites[i + 1].sprite);
            }
            else
            {
                ChangeMemoryAtIndex(memorySprites.Length - 1, null);
            }

            i++;
        }
    }

    public void FlushMemories()
    {
        int i = 0;
        foreach (Image im in memorySprites)
        {

            ChangeMemoryAtIndex(i, null);
            i++;
        }

        memoryCount = 0;
    }
}

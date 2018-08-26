using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingMemory : MonoBehaviour
{
    public float minAnimationMultiplier;
    public float maxAnimationMultiplier;

    // Use this for initialization
    void Start()
    {
        Animator anim = GetComponent<Animator>();

        // Set random speed
        anim.speed = anim.speed * Random.Range(minAnimationMultiplier, maxAnimationMultiplier);

        // Choose random direction
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}

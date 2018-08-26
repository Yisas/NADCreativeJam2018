using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingMemory : MonoBehaviour
{
    public float minAnimationMultiplier;
    public float maxAnimationMultiplier;
    public float selfDestructInterval = 5.0f;

    private float selfDestructTimer = 0;

    private void Awake()
    {
        selfDestructTimer = selfDestructInterval;
    }

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

    private void Update()
    {
        selfDestructTimer -= Time.deltaTime;

        if (selfDestructTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}

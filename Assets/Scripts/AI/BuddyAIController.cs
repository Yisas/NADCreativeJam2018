using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyAIController : MonoBehaviour {
    public GameObject target;
    public float damping = 3;

    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, target.transform.position.x, Time.deltaTime * damping),
                                               Mathf.Lerp(this.transform.position.y, target.transform.position.y, Time.deltaTime * damping), 0);
    }
}

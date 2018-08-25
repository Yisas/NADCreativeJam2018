using UnityEngine;
using System.Collections;

public class CustomCameraFollow : MonoBehaviour
{
    public GameObject player;       //Public variable to store a reference to the player game object
    private float damping = 20;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //transform.position = player.transform.position;
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        Vector3 target = player.transform.position + offset;
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = 
            new Vector3(Mathf.Lerp(this.transform.position.x, target.x, Time.deltaTime * damping),
                                               Mathf.Lerp(this.transform.position.y, target.y, Time.deltaTime * damping), transform.position.z);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls the movement of the bullets
public class bullet : MonoBehaviour
{
    public Vector3 bulletPosition;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;

    //set a maximum speed for the bullet
    public float maxSpeed = 25f;

    //grab the player object
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        //find the player object and set the bullet's position to where the player is
        player = GameObject.Find("player");
        bulletPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //set velocity using player's rotation and the max speed
        velocity = player.transform.right * maxSpeed;
        //move the bullet, 1.5 is just to make it a little faster
        bulletPosition += velocity * 1.5f * Time.deltaTime;

        //set the actual position to the bulletPosition
        transform.position = bulletPosition;
    }
}


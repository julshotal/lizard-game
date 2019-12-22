using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls enemy movement using forces 
public class enemy : MonoBehaviour {

    public Vector3 enemyPosition;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;
    public float mass;

    void Start () {
        //set enemy posiiton to object's posiiton
        enemyPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        //apply a counterforce to push objects along
        //can be negative or positive for a larger range of movement
        Vector3 counterForce = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 1);
        ApplyForce(counterForce);

        //bigger enemies have a larger mass than the smaller ones
        if(gameObject.tag == "bigboy")
        {
            mass = 3;
        } else if (gameObject.tag == "baby")
        {
            mass = 1;
        }

        //use the time it takes to run each frame to set the velocity and the new position
        velocity += acceleration * Time.deltaTime;
        enemyPosition += velocity * Time.deltaTime;

        //normalize the direction vectore, set acceleration to 0, and clamp the maginute so they don't speed up infintely
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        velocity = Vector3.ClampMagnitude(velocity, 1.5f);

        //make sure the enemy position z stays 1 so they don't slip under other objects
        enemyPosition.z = 1;
        transform.position = enemyPosition;

        //wrap enemies across screen
        Wrap();
    }

    //apply force to the objects, the higher the mass the slower the acceleration
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    //wrap the enemy objects around the screen
    public void Wrap()
    {
        //get camera and it's height and width
        Camera myCamera = Camera.main;
        float camHeight = myCamera.orthographicSize;
        float camWidth = camHeight * myCamera.aspect;

        //wrap left to right
        if (enemyPosition.x > camWidth)
        {
            enemyPosition.x = -camWidth;
        }
        else if (enemyPosition.x < -camWidth)
        {
            enemyPosition.x = camWidth;
        }

        //wrap up and down
        if (enemyPosition.y > camHeight)
        {
            enemyPosition.y = -camHeight;
        }
        else if (enemyPosition.y < -camHeight)
        {
            enemyPosition.y = camHeight;
        }
    }
}

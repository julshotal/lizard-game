using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses AABB collision and circle collision to detect collision between two objects
public class CollisionDetection : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    //bounding box collision
    //this is used for collision between the player and enemies
    public bool AABBCollision(SpriteRenderer pCollider, SpriteRenderer eCollider)
    {
        //if the boxes of both gameobjects collide for all four bounds, return true
        if (eCollider.bounds.min.x < pCollider.bounds.max.x && eCollider.bounds.max.x > pCollider.bounds.min.x && eCollider.bounds.max.y > pCollider.bounds.min.y && eCollider.bounds.min.y < pCollider.bounds.max.y)
        {
            return true;
        } else
        {
            return false;
        }
    }

    //circle collision
    //this is used for the collision between the bullets and enemies
    public bool CircleCollision(SpriteRenderer pCollider, SpriteRenderer eCollider)
    {
        //calculate the distance between center points using the pythagorean theorm
        float distance = Mathf.Pow((pCollider.bounds.center.x - eCollider.bounds.center.x), 2) + Mathf.Pow((pCollider.bounds.center.y - eCollider.bounds.center.y), 2);
        distance = Mathf.Sqrt(distance);

        //if the distance between center points is less than the addition of both radi, return true
        if (distance < pCollider.bounds.extents.x + eCollider.bounds.extents.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

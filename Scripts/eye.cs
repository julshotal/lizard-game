using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rotates the eye on the win screen just for fun
//this is from the RotateHand from the clock exercise
public class eye : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //get the mouse position and turn it to a world points
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //set the angle based on mouse x and y
        float angleOfRotation = (Mathf.Atan2(-mouseWorldPos.x, mouseWorldPos.y) * Mathf.Rad2Deg);

        //Debug.Log(angleOfRotation);
        transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);
    }
}

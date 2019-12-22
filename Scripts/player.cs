using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//controls all player movements and shooting functionality
public class player : MonoBehaviour {


    public float accelRate;                     // Small, constant rate of acceleration
    public Vector3 vehiclePosition;             // Local vector for movement calculation
    public Vector3 direction;                   // Way the vehicle should move
    public Vector3 velocity;                    // Change in X and Y
    public Vector3 acceleration;                // Small accel vector that's added to velocity
    public float angleOfRotation;               // 0 
    public float maxSpeed;                      // 0.3 per frame, limits mag of velocity

    public GameObject bullet; //grab the bullet gameobject
    public Vector3 bulletPosition; //to set the positon to the same as the player

    //two lists, one for all the shots fired and the other to store ones that have hit/left the screen
    public List<GameObject> shotsFired = new List<GameObject>();
    public List<GameObject> destroyBullets = new List<GameObject>();

    //.5 delay on shooting, timer must increment to .65 before player can shoot again
    private float shootWait = .65f;
    private float timer = 0;

    // Use this for initialization
    void Start () {

        //start the player at 0,0, facing right, and not moving
        vehiclePosition = new Vector3(0, 0, 1);     
        direction = new Vector3(1, 0, 1);           
        velocity = new Vector3(0, 0, 1);            
    }
	
	// Update is called once per frame
	void Update () {
        //wrap around the screen
        Wrap();

        //move forwards when up arrow is pressed
        Accelerate();

        //rotate on left and right
        RotateVehicle();

        //general movement once player begins accelerating
        Drive();

        //changes and sets the transform component
        SetTransform();

        //shoots the bullet
        Shoot();

        //increments the timer to prevent rapid fire
        timer += Time.deltaTime;
    }

 
    //changes the position and sets it equal to the changing vehiclePosition
    public void SetTransform()
    {
        // Rotate vehicle sprite
        transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);

        // Set the transform position
        transform.position = vehiclePosition;
    }


    //handles the math to move the vehicle once acceleration is called using the up arrow
    public void Drive()
    {
        // Accelerate
        // Small vector that's added to velocity every frame
        acceleration = accelRate * direction;

        // Limit velocity so it doesn't become too large
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Add velocity to vehicle's position
        vehiclePosition += velocity;
    }

    //fires the bullets
    public void Shoot()
    {
        //set the bullet position to where the vehicle is
        bulletPosition = new Vector3(vehiclePosition.x, vehiclePosition.y, 1);

        //if the spacebar is pressed and the timer is up
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(timer >= shootWait)
            {
                //instantiate a bullet at the player's location and add it to the list of bullets
                //timer is reset to 0
                GameObject bulletShot = Instantiate(bullet, bulletPosition, Quaternion.identity);
                shotsFired.Add(bulletShot);
                timer = 0;
            }
        }
    }

    //rotates the vehicle using the left and right keys
    public void RotateVehicle()
    {
        // Player can control direction
        // Left arrow key = rotate left by 2 degrees
        // Right arrow key = rotate right by 2 degrees
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angleOfRotation += 2;
            direction = Quaternion.Euler(0, 0, 2) * direction;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            angleOfRotation -= 2;
            direction = Quaternion.Euler(0, 0, -2) * direction;
        }
    }

    //when the up arrow is pressed, the vehicle accelerates
    public void Accelerate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += acceleration;
        }
        else
        {
            //when the arrow is let up it deccelrates to a stop
            velocity *= .95f;
        }
    }

    public void Wrap()
    {
        //get camera and it's height and width
        Camera myCamera = Camera.main;
        float camHeight = myCamera.orthographicSize;
        float camWidth = camHeight * myCamera.aspect;

        //wrap left to right
        if (vehiclePosition.x > camWidth)
        {
            
            vehiclePosition.x = -camWidth;
        }
        else if (vehiclePosition.x < -camWidth)
        {
            vehiclePosition.x = camWidth;
        }

        //wrap up and down
        if (vehiclePosition.y > camHeight)
        {
            vehiclePosition.y = -camHeight;
        }
        else if (vehiclePosition.y < -camHeight)
        {
            vehiclePosition.y = camHeight;
        }
    }
}

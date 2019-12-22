using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all collisions happening in game
//also handles incrementing the score/decrementing lives
//destroys any gameobjects that are shot or when bullets go offscreen
public class CollisionManager : MonoBehaviour
{

    //enemy/small enemy and player gameobjects, and scene manager to get script
    public GameObject player;
    public GameObject manager;
    public player playerScript;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject small1;
    public GameObject small2;
    public GameObject small3;
    public enemy enemyScript;

    //list to hold all enemy objects
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> smallEnemies = new List<GameObject>();
    List<GameObject> removeEnemies = new List<GameObject>();

    //varaible to hold the collision script
    private CollisionDetection runCollision;

    //variables to hold the renderers of player and enemy gameobjects
    private SpriteRenderer pRenderer;
    private SpriteRenderer eRenderer;
    private SpriteRenderer bRenderer;

    //grabbing the script that controls score and lives
    public UI lives;

    //prevent the life counter from completely depleting on one hit
    public float collisionWait = .5f;
    public float timer = 0;

    // Use this for initialization
    void Start()
    {

        //grab the UI script and the player script
        lives = GetComponent<UI>();
        playerScript = player.GetComponent<player>();

        //get camera and it's height and width
        Camera myCamera = Camera.main;
        float camHeight = myCamera.orthographicSize;
        float camWidth = camHeight * myCamera.aspect;

        //intialize 10 different gameobjects using the 3 unique enemies
        //these are scattered randomly amongst the scene
        for (int i = 0; i < 10; i++)
        {
            if (i <= 3)
            {
                GameObject newEnemy = Instantiate(enemy1, new Vector3(Random.Range(-camWidth, camWidth), Random.Range(-camHeight, camHeight), 1), Quaternion.identity);
                enemies.Add(newEnemy);
            }
            else if (i > 3 && i <= 6)
            {
                GameObject newEnemy = Instantiate(enemy2, new Vector3(Random.Range(-camWidth, camWidth), Random.Range(-camHeight, camHeight), 1), Quaternion.identity);
                enemies.Add(newEnemy);
            }
            else if (i > 6)
            {
                GameObject newEnemy = Instantiate(enemy3, new Vector3(Random.Range(-camWidth, camWidth), Random.Range(-camHeight, camHeight), 1), Quaternion.identity);
                enemies.Add(newEnemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //increment timer
        timer += Time.deltaTime;

        //get the CollisionDetection script
        runCollision = manager.GetComponent<CollisionDetection>();

        //add the list of smaller enemies to the enemy list and clear it
        //this simplifies collision for both types of enemies
        enemies.AddRange(smallEnemies);
        smallEnemies.Clear();

        //run the function that removes all hit enemies
        //run the function that removes bullets that have hit/gone off screen
        RemoveEnemy();
        RemoveBullets();

        //for all enemies and bullets contained on the screen, check if 1
        //1) the enemies are colliding with the player
        //2) the bullets are colliding with the enemies
        foreach (GameObject enemy in enemies)
        {
            //call AABB collision for asterioid vs player collision
            CheckCollisionAABB(enemy, player);
            enemyScript = enemy.GetComponent<enemy>();

            Vector3 position = enemyScript.enemyPosition;

            foreach (GameObject bullet in playerScript.shotsFired)
            {
                CheckCollisionBullet(enemy, bullet, position);
                //check if the bullet is out of the screen, if it is, it's removed
                Remove(bullet);
            }
        }

    }

    //passes in gameobjects and renderers to AABB collision and changes color if true
    private void CheckCollisionAABB(GameObject enemy, GameObject player)
    {
        pRenderer = player.GetComponent<SpriteRenderer>();
        eRenderer = enemy.GetComponent<SpriteRenderer>();

        eRenderer.color = Color.white;


        if (runCollision.AABBCollision(pRenderer, eRenderer))
        {
            eRenderer.color = Color.red;

            //delay the life counter decreasing
            if(timer >= collisionWait)
            {
                lives.hits++;
                timer = 0;
            }
        } 
    }

    //check whether the bullet and enemies are colliding, position is used to istantiate the small enemies
    private void CheckCollisionBullet(GameObject enemy, GameObject bullet, Vector3 position)
    {
        //grab the enemy and bullet colliders
        bRenderer = bullet.GetComponent<SpriteRenderer>();
        eRenderer = enemy.GetComponent<SpriteRenderer>();

        //if the bullet and enemy are colliding (using bounding circles)
        if (runCollision.CircleCollision(bRenderer, eRenderer))
        {
            //destroy the bullet, this limits them to only hitting a single enemy at a time
            //before, you could just hit 3-4 enemies at a time
            playerScript.destroyBullets.Add(bullet);

            //if the enemy is larger, depending on what type it is plit it into 3 smaller versions of itself
            //the large object is them sent to the destroy script to be destroyed
            if(enemy.tag == "bigboy" )
            {
                if(enemy.name == "enemy1(Clone)")
                {
                    removeEnemies.Add(enemy);
                    SplitEnemy(small1, position);
                }

                if (enemy.name == "enemy2(Clone)")
                { 
                    removeEnemies.Add(enemy);
                    SplitEnemy(small2, position);
                }

                if (enemy.name == "enemy3(Clone)")
                {
                    removeEnemies.Add(enemy);
                    SplitEnemy(small3, position);
                }

                //add 20 to the score for hitting a large object
                lives.score += 20;

            }

            //if the enemy is a small enemy, slate it for destruction and add 50 to the score
            if (enemy.tag == "baby")
            {
                removeEnemies.Add(enemy);
                lives.score += 50;
            }
        }
    }

    //function to split larger enemies into smaller ones
    private void SplitEnemy(GameObject type, Vector3 position)
    {
        //istantiate 3 small enemies from the position of the larger one
        //send them to the small enemies script, which will merge with the enemies script in update
        for(int i = 0; i < 3; i++)
        {
            GameObject newSmall1 = Instantiate(type, position, Quaternion.identity);
            smallEnemies.Add(newSmall1);
        }
    }

    //destruction scripts
    //Remove hit enemies from the enemies list
    //then, destroy them
    private void RemoveEnemy()
    {
        foreach (GameObject hitEnemy in removeEnemies)
        {
            enemies.Remove(hitEnemy);
        }

        for (int i = 0; i < removeEnemies.Count; i++)
        {
            Destroy(removeEnemies[i]);
        }
    }

    //remove either hit or offscreen bullets from the shotsfired list
    //then, destroy them
    private void RemoveBullets()
    {
        foreach (GameObject goneBullets in playerScript.destroyBullets)
        {
            playerScript.shotsFired.Remove(goneBullets);
        }

        for (int i = 0; i < playerScript.destroyBullets.Count; i++)
        {
            Destroy(playerScript.destroyBullets[i]);
        }
    }

    //check if a fired bullet is offscreen
    //if it is, send it to the destruction list
    public void Remove(GameObject currentBullet)
    {
        //get camera and it's height and width
        Camera myCamera = Camera.main;
        float camHeight = myCamera.orthographicSize;
        float camWidth = camHeight * myCamera.aspect;

        //wrap left to right
        if (currentBullet.transform.position.x > camWidth || currentBullet.transform.position.x < -camWidth || currentBullet.transform.position.y > camHeight || currentBullet.transform.position.y < -camHeight)
        {
            playerScript.destroyBullets.Add(currentBullet);
        }
    }
}

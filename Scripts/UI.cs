using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//controls the lives and score compenents of the game
public class UI : MonoBehaviour {
    /*shoutout to this guy for helping a lot: https://www.youtube.com/watch?v=LsUiJItfzxU*/

    //grab the gameobjects representing the lives
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    //set a hit counter and intialize score to 0
    public int hits = 0;
    public int score = 0;

    //set a boolean to check if a hit has already been registered
    public bool checkHit;

    // Use this for initialization
    void Start () {

        //all 3 lives are active
        life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

        //check how often the player has been hit
        //depending on how many hits, life objects are set to unactive to show how many lives are left
        switch(hits)
            {
                case 1:
                    life1.gameObject.SetActive(true);
                    life2.gameObject.SetActive(true);
                    life3.gameObject.SetActive(false);
                    break;
                case 2:
                    life1.gameObject.SetActive(true);
                    life2.gameObject.SetActive(false);
                    life3.gameObject.SetActive(false);
                    break;
                case 3:
                    life1.gameObject.SetActive(false);
                    life2.gameObject.SetActive(false);
                    life3.gameObject.SetActive(false);
                    SceneManager.LoadScene("GameOver");
                    break;
            }

        //check if the score reaches the maximum it can (ie: all enemies are destroyed)
        //there are 10 enemies (200 points) that can break into 3 babies each (1,500 points)
        //go to the screen for winners
        if(score >= 1700)
        {
            SceneManager.LoadScene("YouWin");
        }
	}

    //GUI to display score above lives
    private void OnGUI()
    {
        GUI.color = Color.green;
        GUI.skin.box.fontSize = 20;
        GUI.Box(new Rect(10, 10, 150, 30), "Score: " + score);
        GUI.skin.box.wordWrap = true;
    }
}

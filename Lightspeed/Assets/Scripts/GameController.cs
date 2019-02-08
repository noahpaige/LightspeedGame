using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject WinText;
    public GameObject StartText;
    public Text TimeText;
    public GameObject LoseText;
    public GameObject Player;

    private bool gameStarted = false;
    private bool gameEnded = false;
    private float time = 120;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(time < 0f)
        {
            LoseText.SetActive(true);
            Player.SetActive(false);
        }
        if (!gameStarted)
        {
            if(Input.GetKey("space"))
            {
                StartText.SetActive(false);
                Player.SetActive(true);
                gameStarted = true;
            }

        }
        TimeText.text = "Time: " + ((int)time).ToString();
        if (gameStarted && !gameEnded)
        {
            time -= Time.deltaTime;
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	public void PlayerWon()
    {
        WinText.SetActive(true);
        Player.SetActive(false);
        gameEnded = true;

    }
}

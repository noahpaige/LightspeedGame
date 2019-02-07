using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject WinText;


	// Use this for initialization
	void Start () {
		
	}
	
	public void PlayerWon()
    {
        WinText.SetActive(true);
    }
}

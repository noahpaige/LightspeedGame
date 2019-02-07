using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCheckController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            GameController.instance.PlayerWon();
            Debug.Log("Player won");
        }
    }
}

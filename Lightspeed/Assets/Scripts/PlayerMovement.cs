using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        //Debug.Log("PlayerMovement.Update -- horizontalMove: " + horizontalMove);
		
	}

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}

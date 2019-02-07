using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private Animator ani;
    private Rigidbody2D rb2d;
    private bool isJumping = false;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vel = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        ani.SetFloat("VelocityX", vel.x);
        ani.SetFloat("VelocityY", vel.y);
        ani.SetBool("isJumping", isJumping);

    }

    public void SetIsJumping(bool b)
    {
        isJumping = b;
        //Debug.Log("Jumping: " + isJumping);
    }
}

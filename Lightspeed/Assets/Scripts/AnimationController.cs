using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public  float animSpeedFactor = 1f;
    private Animator ani;
    private Rigidbody2D rb2d;
    private bool isJumping = false;
    private LightContainerController lcon;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        lcon = transform.Find("LightContainer").GetComponent<LightContainerController>();

	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vel = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        ani.SetFloat("AnimSpeed", lcon.CalculateAnimationSpeed() * animSpeedFactor);
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

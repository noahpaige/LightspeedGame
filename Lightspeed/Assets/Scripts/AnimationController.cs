using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private new Animator animator;
    private bool isJumping = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vel = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        animator.SetFloat("VelocityX", vel.x);
        animator.SetFloat("VelocityY", vel.y);
        animator.SetBool("isJumping", isJumping);

    }

    public void SetIsJumping(bool b)
    {
        isJumping = b;
    }
}

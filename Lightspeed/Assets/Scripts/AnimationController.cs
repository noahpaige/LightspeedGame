using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private new Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        if (Input.GetAxis("Vertical") != 0)
            animator.SetBool("isJumping", true);
        else
            animator.SetBool("isJumping", false);

    }
}

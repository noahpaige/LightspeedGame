using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private new Animation animation;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0)
            animation.CrossFade("Player Running 2");
        else
            animation.CrossFade("Player Idle 2");
    }
}

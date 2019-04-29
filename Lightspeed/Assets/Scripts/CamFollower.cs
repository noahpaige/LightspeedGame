using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour {

    public GameObject player;

    public float smoothSpeed = 0.125f;
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 desiredPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        
    }
}

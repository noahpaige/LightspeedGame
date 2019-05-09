using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    public GameObject player;

    [Range(1,128)]
    public int moveFactor = 8;

    private Vector3 prevPosition;

	// Use this for initialization
	void Start () {
        prevPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toPos = transform.position + (player.transform.position - prevPosition);
        transform.position = Vector3.Lerp(transform.position, toPos, 1f / (float)moveFactor);

        prevPosition = player.transform.position;
	}
}

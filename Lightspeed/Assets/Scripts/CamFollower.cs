﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour {

    private GameObject player;

    public float smoothSpeed = 0.125f;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(player == null) player = GameObject.Find("Player");
        Vector3 desiredPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        
    }
}

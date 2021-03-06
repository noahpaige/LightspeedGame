﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEvent : MonoBehaviour {

    public UnityEvent e;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(e != null)
        {
            Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Triggered finish");
            e.Invoke();
        }
    }

    public void CallPlayerWon()
    {
        GameController.instance.PlayerWon();
    }
}

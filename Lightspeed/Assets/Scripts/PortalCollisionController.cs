using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollisionController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.instance.GoToNextScene();
    }
}

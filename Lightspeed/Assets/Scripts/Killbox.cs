using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour {

    public GameObject platforms;
    public GameObject spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().position = spawnPoint.transform.position;
            foreach (Transform child in platforms.transform)
            {
                child.gameObject.GetComponent<PhysicsPlatformMovementController>().ResetPosition();
            }
        }
    }
}

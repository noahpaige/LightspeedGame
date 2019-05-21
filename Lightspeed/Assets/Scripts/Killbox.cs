using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour {

    private GameObject platforms;
    private GameObject spawnPoint;

    private void Start()
    {
        platforms = GameObject.Find("Platforms");
        spawnPoint = GameObject.Find("spawnpoint");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().position = spawnPoint.transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            foreach (Transform child in platforms.transform)
            {
                if(child.gameObject.activeSelf)
                    child.gameObject.GetComponent<PhysicsPlatformMovementController>().ResetPosition();
            }

            List<GameObject> lights = collision.transform.Find("LightContainer").GetComponent<LightContainerController>().GetLights();
            foreach(GameObject light in lights)
            {
                if (light.activeSelf)
                    light.GetComponent<LightController>().ResetPosition();
            }
                collision.transform.Find("LightContainer").GetComponent<LightContainerController>().ClearLights();
        }
    }

    
}

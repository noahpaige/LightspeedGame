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

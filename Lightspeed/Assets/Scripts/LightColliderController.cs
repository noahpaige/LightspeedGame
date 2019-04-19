using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColliderController : MonoBehaviour
{
    public GameObject container;
    public GameObject rays;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //lcon.SendMessage("AddLight", this.gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
            container.GetComponent<LightContainerController>().AddLight(this.gameObject);
        }
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LightController : MonoBehaviour
{
    public GameObject container;
    public GameObject rays;

    private Transform lightDaddy;
    private Vector3 originalPosition;
    private Quaternion originalQuat;
    private Vector3 originalScale;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //lcon.SendMessage("AddLight", this.gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
            container.GetComponent<LightContainerController>().AddLight(this.gameObject);
        }
    }
    
    public void ResetPosition()
    {
        //transform.SetParent(lightDaddy);
        transform.SetPositionAndRotation(originalPosition, originalQuat);
        transform.localScale = originalScale;
        rays.transform.localScale = originalScale;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Start()
    {
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originalQuat = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        lightDaddy = transform.parent;
    }

}

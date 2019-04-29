using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LightController : MonoBehaviour
{
    public GameObject container;
    public GameObject rays;
    public GameObject trails;

    private Transform lightDaddy;
    private Vector3 originalPosition;
    private Quaternion originalQuat;
    private Vector3 originalScale;

    [SerializeField] [Range(0f, 1f)]private float minMoveSpeed;
    [SerializeField] [Range(0f, 1f)]private float maxMoveSpeed;

    [HideInInspector] public float moveSpeed;

    [Header("Events")]
    [Space]

    public UnityEvent OnCollectEvent;

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
        trails.transform.localScale = originalScale;
        var main = rays.GetComponent<ParticleSystem>().main;
        var c = main.startColor;
        main.startColor = new Color(c.color.r, c.color.g, c.color.b, 255);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Start()
    {
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originalQuat = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        lightDaddy = transform.parent;
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

}

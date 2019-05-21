using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LightController : MonoBehaviour
{
    public GameObject rays;
    public GameObject trails;
    public float      decayTime = 10f;

    private GameObject container;
    private Transform  lightDaddy;
    private Vector3    originalPosition;
    private Quaternion originalQuat;
    private Vector3    originalScale;
    private float      originalDecay;
    private bool       collected = false;

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
            collected = true;
        }
    }

    public void DecayUpdate()
    {
        if (collected)
        {
            bool isDecayed = Decay();
            if (isDecayed)
            {
                container.GetComponent<LightContainerController>().RemoveLight(this.gameObject);
                ResetPosition();
                decayTime = originalDecay;
            }
        }
    }

    public void ResetPosition()
    {
        //transform.SetParent(lightDaddy);
        transform.SetPositionAndRotation(originalPosition, originalQuat);

        transform.localScale        = originalScale;
        rays.transform.localScale   = originalScale;
        trails.transform.localScale = originalScale;

        var main        = rays.GetComponent<ParticleSystem>().main;
        var c           = main.startColor;
        main.startColor = new Color(c.color.r, c.color.g, c.color.b, 255);

        GetComponent<CircleCollider2D>().enabled = true;
        collected = false;
        decayTime = originalDecay;
    }

    private void Start()
    {
        container        = GameObject.Find("Player").transform.Find("LightContainer").gameObject;
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originalQuat     = Quaternion.identity; //new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        originalScale    = Vector3.one;         //new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        lightDaddy       = transform.parent;
        moveSpeed        = Random.Range(minMoveSpeed, maxMoveSpeed);
        originalDecay    = decayTime;
    }

    public bool Decay()
    {
        decayTime -= Time.deltaTime;
        if (decayTime < 0f) return true;
        return false;
    }

    public float GetDecayAmount()
    {
        return decayTime / originalDecay;
    }

    //reduces the radius of the particles emitted over time
    private void AdjustEmitRadius()
    {
        var shape = rays.GetComponent<ParticleSystem>().shape;
        shape.arc = 360f * (decayTime / originalDecay);
    }

    public void ResetDecayTime()
    {
        decayTime = originalDecay;
    }
}

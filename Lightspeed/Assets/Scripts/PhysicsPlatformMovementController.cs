using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicsPlatformMovementController : MonoBehaviour {

    // Don't change these. They are used by DropdownEditor.cs.
    [HideInInspector] public bool followX;
    [HideInInspector] public int dropDownIndex;
    [HideInInspector] public string[] followXorY = new string[] { "follow X", "follow Y"};


    public Vector2 fromPoint;
    public Vector2 toPoint;
    public float startMovingPoint;
    public float endMovingPoint;
    public GameObject player;
    public float maxSpeed = 5f;
    public float drag = 0.5f;

    private Rigidbody2D rb;

    private Vector2 prevPlayerPosition;
    private float speedFactor;

    //private static readonly float EPSILON = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.drag = drag;

        transform.position = fromPoint;
        speedFactor = Mathf.Max(500f, rb.mass);
    }

    void FixedUpdate()
    {
        MovePlatform(player.GetComponent<Rigidbody2D>().position);
        ScaleSpeed();
    }

    private void MovePlatform(Vector2 playerPos)
    {
        float percentageWithinWindow;
        if(followX) percentageWithinWindow = (playerPos.x - startMovingPoint) / (endMovingPoint - startMovingPoint);
        else        percentageWithinWindow = (playerPos.y - startMovingPoint) / (endMovingPoint - startMovingPoint);

        Vector2 targetPoint = Vector2.Lerp(fromPoint, toPoint, percentageWithinWindow);

        Vector2 fromPlatToTarget = targetPoint - rb.position;

        rb.AddForce(fromPlatToTarget * speedFactor);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void ScaleSpeed()
    {
        Vector2 dir = rb.velocity.normalized;
        float halfDistFromTo = Vector2.Distance(fromPoint, toPoint) / 2f;
        float curDist = Mathf.Min(Vector2.Distance(fromPoint, rb.position), Vector2.Distance(toPoint, rb.position));
        float scaledSpeed = curDist / halfDistFromTo;

        rb.velocity = dir * Mathf.Min(scaledSpeed, rb.velocity.magnitude);
    }
    public void ResetPosition()
    {
        rb.position = fromPoint;
    }

    private void OnDrawGizmosSelected()
    {
        float gizWidth = 0.1f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(fromPoint.x, fromPoint.y, -1f), new Vector3(toPoint.x, toPoint.y, -1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(fromPoint.x, fromPoint.y, -1f), gizWidth);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(toPoint.x, toPoint.y, -1f), gizWidth);
    }
}

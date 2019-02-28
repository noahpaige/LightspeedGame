using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Range(0f, Mathf.Infinity)] public float speedFactor = 100f;
    public float maxSpeed = 5f;
    public float drag = 0.5f;

    private Rigidbody2D rb;

    private Vector2 prevPlayerPosition;
    private bool playerWasInWindow = false;
    private float windowDist;
    private float travelDist;


    private static readonly float EPSILON = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        transform.position = fromPoint;
        windowDist = Mathf.Abs(endMovingPoint - startMovingPoint);
        travelDist = Vector2.Distance(fromPoint, toPoint);
    }

    void FixedUpdate()
    {
        //remember to cap speed
        //if player is in window
        // if player was in window
        //add force relative to previous position
        // else add some force idk lol

        if (followX) MovePlatformFollowX2(new Vector2(player.transform.position.x, player.transform.position.y));

    }

    private void MovePlatformFollowX2(Vector2 playerPos)
    {
        float percentageWithinWindow = (playerPos.x - startMovingPoint) / endMovingPoint;
        Vector2 targetPoint = Vector2.Lerp(fromPoint, toPoint, percentageWithinWindow);

        Vector2 fromPlatToTarget = targetPoint - rb.position;

        rb.AddForce(fromPlatToTarget * rb.mass);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void MovePlatformFollowX(Vector2 playerPos)
    {
        if (playerPos.x >= startMovingPoint && playerPos.x <= endMovingPoint)
        {
            if (playerWasInWindow)
            {
                float frameMovement = playerPos.x - prevPlayerPosition.x;
                if (System.Math.Abs(frameMovement) < EPSILON) return;
                if(!IsPlatformMovingWithPlayer(frameMovement))
                {
                    Debug.Log("Not moving with player");
                    //rb.velocity = Vector2.zero;
                }
                AddForceCapped(frameMovement);
            }
            playerWasInWindow = true;
            prevPlayerPosition = playerPos;
        }
        else playerWasInWindow = false;
    }

    bool IsPlatformMovingWithPlayer(float frameMovement)
    {
        bool platformMovingTowardsToPoint = (rb.velocity.normalized == (toPoint - fromPoint).normalized);

        if (platformMovingTowardsToPoint) return frameMovement > 0;
        else                              return frameMovement <= 0;
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

    private void AddForceCapped(float frameMovement)
    {
        Vector2 platformVelocity = rb.velocity;
        Vector2 addForce = (toPoint - fromPoint).normalized * frameMovement * speedFactor * Time.deltaTime;
        rb.AddForce(addForce);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }   

}

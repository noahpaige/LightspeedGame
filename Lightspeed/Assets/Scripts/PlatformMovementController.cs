using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementController : MonoBehaviour
{
    public Vector2[] points; // list of points to lerp along
    public float startMovingPoint;
    public float endMovingPoint;
    public GameObject player;
    public float ease;
    public float tileWidth = 0.19531f;
    private Rigidbody2D rb;

    private int fromPoint;
    private int toPoint;
    public float prevPercentage = 0f; // the % representing where the player is between startMovingPoint and endMovingPoint

    private bool playerWasInWindow = false;
    private Vector2 prevPlayerPos = Vector2.zero;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (points.Length > 1)
        {
            fromPoint = 0;
            toPoint = 1;
        }
        transform.position = new Vector3(points[fromPoint].x, points[fromPoint].y, transform.position.z);

    }

    void FixedUpdate()
    {
        rb.MovePosition(Ease(CalcX(player.transform.position)));
        prevPercentage = Mathf.Max(prevPercentage, 0f);
    }

    Vector2 CalcX(Vector2 playerPos)
    {
        //check if player is in window
        Vector2 output = transform.position;
        if (playerPos.x >= startMovingPoint && playerPos.x <= endMovingPoint)
        {
            if (playerWasInWindow)
            {
                //percentage = percentage player has moved inside window based on current player and previous player positions
                float playerMoveDist = playerPos.x - prevPlayerPos.x;
                float percentage = playerMoveDist / Mathf.Abs(startMovingPoint - endMovingPoint);
                percentage += prevPercentage;
                output = TryMovePlatform(percentage);
            }
            prevPlayerPos = playerPos;
            playerWasInWindow = true;
        }
        else playerWasInWindow = false;


        return output;
    }
    //takes overall percentage
    Vector2 TryMovePlatform(float percentage)
    {
        UpdateFromAndTo(percentage);
        float adjustedPercentage = (percentage - (fromPoint / (points.Length - 1))) * (points.Length - 1) - fromPoint; // percentage between fromPoint and toPoint
        Vector2 moveTowards;
        moveTowards = Vector2.Lerp(points[fromPoint], points[toPoint], adjustedPercentage);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(moveTowards, new Vector2(2f * tileWidth, tileWidth), transform.rotation.z);
        float savedPrevPercentage = prevPercentage;
        foreach(Collider2D col in colliders)
        {
            if (col.CompareTag("collidable") && col.gameObject.transform.parent.gameObject != this.gameObject)
            {
                prevPercentage = savedPrevPercentage;
                UpdateFromAndTo(prevPercentage);
                return moveTowards;
            }
            /*
            else if (col.CompareTag("checkObject"))
            {
                Debug.Log("Platform Collided with player");
                if (col.transform.parent.GetComponent<CharacterController2D>().IsPlayerSquished2(col.gameObject))
                {
                    Debug.Log("PLAYER SQUISHED");
                    prevPercentage = savedPrevPercentage;
                    UpdateFromAndTo(prevPercentage);
                    return moveTowards;
                }
                prevPercentage = percentage;
            }
            */
            else prevPercentage = percentage;
        }
        return moveTowards;
    }

   
    // takes the OVERALL percentage
    void UpdateFromAndTo(float percentage)
    {
        if (toPoint < points.Length - 1)
        {
            if (percentage > (float)toPoint / (points.Length - 1))
            {
                fromPoint++;
                toPoint++;
                return;
            }
        }
        if (fromPoint > 0)
        {
            if (percentage < (float)fromPoint / (points.Length - 1))
            {
                fromPoint--;
                toPoint--;
            }
        }
    }

    private Vector2 Ease(Vector3 desiredPos)
    {
        return Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), desiredPos, ease);
    }


    private void OnDrawGizmosSelected()
    {
        float gizWidth = 0.1f;
        Gizmos.color = Color.red;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(new Vector3(points[i].x, points[i].y, -1f), new Vector3(points[i + 1].x, points[i + 1].y, -1f));
        }
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(new Vector3(points[i].x, points[i].y, -1f), gizWidth);
        }
        

    }

    private Transform GetClosestChildToPosition(Vector3 pos)
    {
        Transform closest = transform.GetChild(0);
        foreach(Transform child in transform)
        {
            if (Vector3.Distance(closest.position, pos) > Vector3.Distance(child.position, pos))
                closest = child;
        }
        return closest;
    }
}

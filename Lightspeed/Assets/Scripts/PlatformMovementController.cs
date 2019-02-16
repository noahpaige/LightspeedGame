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
    private Rigidbody2D rb;

    private int fromPoint;
    private int toPoint;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (points.Length > 1)
        {
            fromPoint = 0;
            toPoint = 1;
        }
        transform.position = new Vector3(points[0].x, points[0].y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(Ease(CalcTranslation(player.transform.position)));
        //MoveChildrenRigidbodies(Ease(CalcTranslation(player.transform.position)));
        //transform.position = Ease(CalcTranslation(player.transform.position));
    }

    Vector2 CalcTranslation(Vector2 playerPos)
    {
        float length = Mathf.Abs(startMovingPoint - endMovingPoint);
        float percentage;
        if (startMovingPoint < endMovingPoint) percentage = (player.transform.position.x - startMovingPoint) / length; // OVERALL percentage
        else percentage = (startMovingPoint - player.transform.position.x) / length; // OVERALL percentage
        UpdateFromAndTo(percentage);
        percentage = (percentage - (fromPoint / (points.Length - 1))) * (points.Length - 1) - fromPoint; // percentage between fromPoint and toPoint
        //Debug.Log("ObjectMovementController.updateFromAndTo -- ADJUSTED PERCENTAGE = " + percentage);
        return Vector2.Lerp(points[fromPoint],
                            points[toPoint],
                            percentage);
    }

    // takes the OVERALL percentage
    void UpdateFromAndTo(float percentage)
    {
        if (toPoint < points.Length - 1)
        {
            if (percentage > (float)toPoint / (points.Length - 1))
            {
                //Debug.Log("ObjectMovementController.updateFromAndTo -- Incrementing. " + percentage + " > " + (float)toPoint / (points.Length - 1));
                fromPoint++;
                toPoint++;
                //Debug.Log("ObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                //Debug.Log("ObjectMovementController.updateFromAndTo -- to  : " + toPoint);
                return;
            }
        }
        if (fromPoint > 0)
        {
            if (percentage < (float)fromPoint / (points.Length - 1))
            {
                //Debug.Log("ObjectMovementController.updateFromAndTo -- Decrementing.        " + percentage + " < " + (float)fromPoint / (points.Length - 1));

                fromPoint--;
                toPoint--;
                //Debug.Log("ObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                //Debug.Log("ObjectMovementController.updateFromAndTo -- to  : " + toPoint);
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
        //Gizmos.color = Color.red;

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(points[i].x, points[i].y, -1f), gizWidth);
        }

    }

    private void MoveChildrenRigidbodies(Vector2 pos)
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            rb.MovePosition(pos); // + new Vector2(child.transform.position.x, child.transform.position.y));
        }
    }

}

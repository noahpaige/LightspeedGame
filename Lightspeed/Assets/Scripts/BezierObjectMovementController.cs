using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierObjectMovementController : MonoBehaviour {

    //must be length >= 3 !!!!!!
    public Vector2[] points; // list of points to lerp along
    public float startMovingPoint;
    public float endMovingPoint;
    public GameObject player;

    private int fromPoint;
    private int midPoint;
    private int toPoint;

    public float percentage;

    // Use this for initialization
    void Start()
    {
        if (points.Length > 1)
        {
            fromPoint = 0;
            midPoint = 1;
            toPoint = 2;
        }
        transform.position = new Vector3(points[0].x, points[0].y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CalcTranslation(player.transform.position);
    }

    Vector3 CalcTranslation(Vector2 playerPos)
    {
        float length = Mathf.Abs(startMovingPoint - endMovingPoint);
        //float percentage;
        if (startMovingPoint < endMovingPoint) percentage = (player.transform.position.x - startMovingPoint) / length; // OVERALL percentage
        else percentage = (startMovingPoint - player.transform.position.x) / length; // OVERALL percentage
        UpdateCurrentPoints(percentage);
        percentage = (percentage - (fromPoint / (points.Length - 1))) * (points.Length - 1) - fromPoint; // percentage between fromPoint and toPoint
        //Debug.Log("BezierObjectMovementController.updateFromAndTo -- ADJUSTED PERCENTAGE = " + percentage);
        Vector2 bez = Bezier(percentage, points[fromPoint], points[midPoint], points[toPoint]);
        return new Vector3(bez.x, bez.y, transform.position.z);
    }

    // takes the OVERALL percentage
    void UpdateCurrentPoints(float percentage)
    {
        if (toPoint < points.Length - 1)
        {
            if (percentage > (float)toPoint / (points.Length))
            {
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- Incrementing. " + percentage + " > " + (float)toPoint / (points.Length));
                fromPoint += 3;
                midPoint += 3;
                toPoint += 3;
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- to  : " + toPoint);
                return;
            }
        }
        if (fromPoint > 0)
        {
            if (percentage < (float)fromPoint / (points.Length))
            {
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- Decrementing.        " + percentage + " < " + (float)fromPoint / (points.Length));

                fromPoint -= 3;
                midPoint -= 3;
                toPoint -= 3;
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                //Debug.Log("BezierObjectMovementController.updateFromAndTo -- to  : " + toPoint);
            }
        }
    }

    public Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
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
}

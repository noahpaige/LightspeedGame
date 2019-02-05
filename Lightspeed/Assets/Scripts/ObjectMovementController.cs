using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementController : MonoBehaviour {

    public Vector2[] points; // list of points to lerp along
    public float startMovingPoint;
    public float endMovingPoint;
    public GameObject player;

    private int fromPoint;
    private int toPoint;

    public float percentage;

	// Use this for initialization
	void Start ()
    {
        if(points.Length > 1)
        {
            fromPoint = 0;
            toPoint   = 1;
        }
        transform.position = new Vector3(points[0].x, points[0].y, transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = CalcTranslation(player.transform.position);
	}

    Vector3 CalcTranslation(Vector2 playerPos)
    {
        float length = Mathf.Abs(startMovingPoint - endMovingPoint);
        //float percentage;
        if (startMovingPoint < endMovingPoint) percentage = (player.transform.position.x - startMovingPoint) / length; // OVERALL percentage
        else                                   percentage = (startMovingPoint - player.transform.position.x) / length; // OVERALL percentage
        UpdateFromAndTo(percentage);
        percentage = (percentage - (fromPoint / (points.Length - 1))) * (points.Length - 1) - fromPoint; // percentage between fromPoint and toPoint
        //Debug.Log("ObjectMovementController.updateFromAndTo -- ADJUSTED PERCENTAGE = " + percentage);
        return Vector3.Lerp(new Vector3(points[fromPoint].x, points[fromPoint].y, transform.position.z),
                            new Vector3(points[toPoint].x,   points[toPoint].y,   transform.position.z),
                            percentage);
    }

    // takes the OVERALL percentage
    void UpdateFromAndTo(float percentage)
    {
        if(toPoint < points.Length - 1)
        {
            if(percentage > (float)toPoint / (points.Length - 1))
            {
                Debug.Log("ObjectMovementController.updateFromAndTo -- Incrementing. " + percentage + " > " + (float)toPoint / (points.Length - 1));
                fromPoint++;
                toPoint++;
                Debug.Log("ObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                Debug.Log("ObjectMovementController.updateFromAndTo -- to  : " + toPoint);
                return;
            }
        }
        if(fromPoint > 0)
        {
            if(percentage < (float)fromPoint / (points.Length - 1))
            {
                Debug.Log("ObjectMovementController.updateFromAndTo -- Decrementing.        " + percentage + " < " + (float)fromPoint / (points.Length - 1));
                
                fromPoint--;
                toPoint--;
                Debug.Log("ObjectMovementController.updateFromAndTo -- from: " + fromPoint);
                Debug.Log("ObjectMovementController.updateFromAndTo -- to  : " + toPoint);
            }
        }
    }
}

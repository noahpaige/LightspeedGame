using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    [Range(0f, 1f)] public float xMoveFactor = 0.5f;
    [Range(0f, 1f)] public float yMoveFactor = 0.5f;

    public bool followX = true;
    public bool followY = false;

    //public bool multiplePanels = false;

    private Transform    camTransform;
    private Vector3      prevPosition;
    private int          leftIndex;
    private int          rightIndex;
    private GameObject[] panels;
    private float        xOffset;
    private float        camWidth = 10;

    // Use this for initialization
    void Start () {
        Debug.Log("Parallax child count: " + transform.childCount);
        camTransform = Camera.main.transform;
        camWidth = (2f * Camera.main.orthographicSize) * Camera.main.aspect;
        //Debug.Log("Cam width = " + camWidth);
        prevPosition = camTransform.position;
        panels = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
        }

        Array.Sort(panels, CompareXPos);
        leftIndex = 0;
        rightIndex = panels.Length - 1;

        xOffset = panels[2].transform.position.x;
    }
	
	void LateUpdate ()
    {
        TranslatePanels();
        TryScroll();
	}

    private int CompareXPos(GameObject a, GameObject b)
    {
        return Math.Sign(a.transform.position.x - b.transform.position.x);
    }

    private void TranslatePanels()
    {
        Vector2 toPos = transform.position + (camTransform.position - prevPosition);
        if (!followX) toPos.x = transform.position.x;
        if (!followY) toPos.y = transform.position.y;
        float newX = Mathf.Lerp(transform.position.x, toPos.x, xMoveFactor);
        float newY = Mathf.Lerp(transform.position.y, toPos.y, yMoveFactor);
        transform.position = new Vector3(newX, newY, transform.position.z);

        prevPosition = camTransform.position;
    }


    private void TryScroll()
    {
        if (camTransform.position.x < (panels[leftIndex].transform.position.x + camWidth))
            ScrollLeft();
        if (camTransform.position.x > (panels[rightIndex].transform.position.x - camWidth))
            ScrollRight();
    }

    private void ScrollLeft()
    {
        panels[rightIndex].transform.position = new Vector3(panels[leftIndex].transform.position.x - xOffset,
                                                            panels[rightIndex].transform.position.y,
                                                            panels[rightIndex].transform.position.z);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = panels.Length - 1;
    }

    private void ScrollRight()
    {
        panels[leftIndex].transform.position = new Vector3(panels[rightIndex].transform.position.x + xOffset,
                                                           panels[leftIndex].transform.position.y,
                                                           panels[leftIndex].transform.position.z);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == panels.Length)
            leftIndex = 0;
        Debug.Log("Right Index = " + rightIndex + "       Left Index = " + leftIndex);
    }
}

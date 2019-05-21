using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCanvasScaler : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam == null) cam = Camera.main;
        float height = 2f     * cam.orthographicSize;
        float width  = height * cam.aspect;

        RectTransform rTrans = GetComponent<RectTransform>();
        rTrans.sizeDelta = new Vector2(width, height);
    }
}

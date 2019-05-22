using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMover : MonoBehaviour
{

    [Range(0f, 1f)] public float speed = 0.2f;
    [Range(0f, 10)] public float maxCamYTranslate = 5f;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    [HideInInspector] public Vector2 mouseViewportPos;
    public Light camLight;

    private int leftMask  = 3840; // 1111 0000 0000
    private int midMask   = 240;  // 0000 1111 0000;
    private int rightMask = 15;   // 0000 0000 1111;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseViewportPos = new Vector2(mouseViewportPos.x - 0.5f, (mouseViewportPos.y - 0.5f) * 2f);

        float modifiedSpeed = speed + (speed * mouseViewportPos.x);
        transform.position = new Vector3(transform.position.x + modifiedSpeed, mouseViewportPos.y * maxCamYTranslate, transform.position.z);

        //ColorByMousePosition();

    }

    private void ColorByMousePosition() // does not work XD
    {
        int firstHalf = (int)Mathf.Floor(mouseViewportPos.x * 4095);
        int secondHalf = (int)Mathf.Floor(mouseViewportPos.y * 4095);

        float r = ((firstHalf & leftMask)  + (secondHalf & leftMask))  / 255.0f;
        float g = ((firstHalf & midMask)   + (secondHalf & midMask))   / 255.0f;
        float b = ((firstHalf & rightMask) + (secondHalf & rightMask)) / 255.0f;


        camLight.color = new Color(r, g, b, 1);
    }
}

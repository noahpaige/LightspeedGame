using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerpController : MonoBehaviour {

    public Color Color1;
    public Color Color2;

    public float step = 100f;

    // Use this for initialization
    void Start() {

        foreach (Transform child in transform)
        {
            SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
            float pos = child.transform.position.x;
            SetColor(rend, pos);
        }
    }
	
    void SetColor(SpriteRenderer rend, float pos)
    {
        bool flip = ((int)pos / (int)step) % 2 == 1;
        if (flip) Debug.Log("Flippd");
        else Debug.Log("NOT");
        if (flip)
            rend.color = Color.Lerp(Color2, Color1, Mathf.Abs((pos % step) / step));
        else
            rend.color = Color.Lerp(Color2, Color1, Mathf.Abs((pos % step) / step));
    }
}

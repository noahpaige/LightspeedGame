using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSoundPlayer : MonoBehaviour
{
    [Range(0.1f, 1f)] public float panWidth = 0.5f;

    public AudioSource neonShort;
    public AudioSource neonStatic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseViewportPos = new Vector2(mouseViewportPos.x - 0.5f, (mouseViewportPos.y - 0.5f) * 2f);

        float lerpVal = (1f + mouseViewportPos.x) / 2f;

        neonShort.panStereo  = Mathf.Lerp(-panWidth, panWidth, lerpVal);
        neonStatic.panStereo = Mathf.Lerp(-panWidth, panWidth, lerpVal);
    }
}

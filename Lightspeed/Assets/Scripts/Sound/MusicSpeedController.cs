using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSpeedController : MonoBehaviour
{
    [Range(0f, 1f)] public float minPitch = 0.7f;
    [Range(0f, 3f)] public float maxPitch = 1.2f;


    private AudioSource musicPlayer;
    private LightContainerController lcon;
    private float lerpTime;
    private bool isTransitioningToDifferentPitch = false;
    private float curPitch;
    private int curNumLights = 0;
    private int totalNumLights;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer    = GetComponent<AudioSource>();
        lcon           = GameObject.Find("Player").transform.Find("LightContainer").GetComponent<LightContainerController>();
        lerpTime       = lcon.arrangeTime;
        curPitch       = minPitch;
        totalNumLights = GameObject.Find("Lights").transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        //musicPlayer.pitch = minPitch + ((maxPitch - minPitch) * lcon.CalculateMusicSpeed());

        if(isTransitioningToDifferentPitch)
        {
            if(timer < lerpTime)
            {
                float from = minPitch + (maxPitch - minPitch) * ((float)curNumLights) / totalNumLights;
                float to   = minPitch + (maxPitch - minPitch) * ((float)lcon.GetLightCount()) / totalNumLights;
                musicPlayer.pitch = Mathf.Lerp(from, to, timer);
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f;
                isTransitioningToDifferentPitch = false;
                curNumLights = lcon.GetLightCount();
            }
        }
        else
        {
            musicPlayer.pitch = minPitch + (maxPitch - minPitch) * ((float)curNumLights) / totalNumLights;
        }

        if(curNumLights != lcon.GetLightCount()) isTransitioningToDifferentPitch = true;
        
    }
}

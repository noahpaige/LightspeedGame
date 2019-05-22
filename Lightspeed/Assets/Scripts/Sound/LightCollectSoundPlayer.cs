using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollectSoundPlayer : MonoBehaviour
{

    [Range(0.5f, 1f)] public float minPitch = 0.5f;
    [Range(1f, 3f)]   public float maxPitch = 2f;

    public AudioSource chime;

    public void PlayChime(int numChime)
    {
        int totalLights = GameController.instance.GetNumLightsInCurrentLevel();
        if (numChime > totalLights) numChime = totalLights;
        chime.pitch = Mathf.Lerp(minPitch, maxPitch, (float)numChime / totalLights);
        chime.Play();
    }
}

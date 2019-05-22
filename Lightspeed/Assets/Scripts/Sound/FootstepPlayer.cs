using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [Range(0.001f, 0.5f)] public float volumeMin = 0.001f;
    [Range(0.1f, 1f)]     public float volumeMax = 0.5f;

    [Range(0.1f, 10f)] public float footstepThresholdVelocity = 2f;
    [Range(1, 8)]      public int   lowVolumeFootstepProbabilityFactor = 2;

    public float stepSpeedModifier = 0.2f;

    private CharacterController2D cc;
    private Rigidbody2D rb;
    private AudioSource footstep;
    private AnimationController ac;


    // Start is called before the first frame update
    void Start()
    {
        cc       = transform.parent.GetComponent<CharacterController2D>();
        rb       = transform.parent.GetComponent<Rigidbody2D>();
        footstep = GetComponent<AudioSource>();
        ac       = transform.parent.GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cc.GetIsGrounded() && Mathf.Abs(rb.velocity.x) > footstepThresholdVelocity && !footstep.isPlaying)
        {
            footstep.pitch = stepSpeedModifier * ac.GetAnimSpeed();
            footstep.volume = GetVolumeByFactor(lowVolumeFootstepProbabilityFactor);
            footstep.Play();
        }
    }

    private float GetVolumeByFactor(int factor)
    {
        if (factor == 1) return Random.Range(volumeMin, volumeMax);
        else             return Random.Range(volumeMin, GetVolumeByFactor(factor - 1));
    }
}

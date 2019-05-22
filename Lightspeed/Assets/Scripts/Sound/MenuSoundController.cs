using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    public AudioSource hoverSound;
    public AudioSource selectSound;

    public void PlayHoverSound () { hoverSound.Play();  }
    public void PlaySelectSound() { selectSound.Play(); }
}

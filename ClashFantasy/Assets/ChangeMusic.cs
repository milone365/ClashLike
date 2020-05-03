using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public string musicToPlay;
    private void Start()
    {
        SoundManager.instance.playMusic(musicToPlay);
    } 
}

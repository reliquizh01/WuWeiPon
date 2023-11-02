using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    public AudioSource audioSource;

    private MusicSetContainer currentMusicSet;

    public void PlayAudioTheme(MusicSetContainer newContainer)
    {
        currentMusicSet = newContainer;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    public string musicSetName;

    public AudioSource audioSource;
    public float playSequenceAdjustment = 0.2f;

    private MusicSetContainer currentMusicSet;

    int musicIndex;
    public void SetAndPlay(string musicSet)
    {
        musicIndex = 0;
        musicSetName = musicSet;

        MusicSetContainer newContainer = SoundManager.Instance.GetMusicSet(musicSet);
        currentMusicSet = newContainer;

        Play();

        if (currentMusicSet.musicType == MusicTypeEnum.Sequential)
        {
            StartCoroutine(PlayNext(audioSource.clip.length-playSequenceAdjustment));
        }
        else
        {
            audioSource.loop = currentMusicSet.loop;
        }
    }

    public void Play()
    {
        audioSource.clip = currentMusicSet.musicList[musicIndex];
        audioSource.Play();
    }

    IEnumerator PlayNext(float length)
    {
        yield return new WaitForSeconds(length);
        musicIndex++;

        Play();

        if(musicIndex < currentMusicSet.musicList.Count)
        {
            StartCoroutine(PlayNext(audioSource.clip.length));
        }
        else
        {
            musicIndex = 0;
        }

    }
}

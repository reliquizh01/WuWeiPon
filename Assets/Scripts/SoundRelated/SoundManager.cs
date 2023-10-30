using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Camera mainCam;

    public AudioSource introbackgroundSource;
    public AudioSource loopBackgroundSource;

    public AudioSource specialEffectsSource;

    public List<AudioClip> bladeToBladeClips;
    public List<AudioClip> hiltToHiltClips;
    public List<AudioClip> bladeToHiltClips;

    public List<MusicSetContainer> musicSets;
    private MusicSetContainer currentBackgroundMusicSet;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {

    }

    public void PlayBackgroundTheme(LocationEnum location)
    {
        ResetBackgroundTheme();

        string theme = location.ToString();

        currentBackgroundMusicSet = musicSets.Find(x => x.musicSetName == theme);

        if(currentBackgroundMusicSet.musicList.Count > 1)
        {
            introbackgroundSource.clip = currentBackgroundMusicSet.musicList[0];
            introbackgroundSource.Play();

            loopBackgroundSource.clip = currentBackgroundMusicSet.musicList[1];
            loopBackgroundSource.PlayScheduled(introbackgroundSource.clip.length);
        }
        else if(currentBackgroundMusicSet.musicList.Count == 1)
        {
            loopBackgroundSource.clip = currentBackgroundMusicSet.musicList[0];
            loopBackgroundSource.Play();
        }
    }

    public void ResetBackgroundTheme()
    {
        introbackgroundSource.Stop();
        loopBackgroundSource.Stop();
    }
}

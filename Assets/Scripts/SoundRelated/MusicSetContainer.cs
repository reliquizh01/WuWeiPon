using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicSetContainer
{
    public string musicSetName;
    public MusicTypeEnum musicType;
    public bool loop;
    public List<AudioClip> musicList;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicSetContainer
{
    public string musicSetName;
    public MusicTypeEnum musicType;
    public bool isLoopable;
    public List<AudioClip> musicList;
}

using DataManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using User.Data;

public class DataVaultManager : MonoBehaviour
{
    public static DataVaultManager Instance;

    public List<Sprite> SkillSprites;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    public Sprite GetSkillSprite(string skillName)
    {
        return SkillSprites.First(x => x.name == skillName);
    }

}

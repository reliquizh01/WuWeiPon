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

    public List<CurrencySpriteItem> CurrencySprites;

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

    #region Currency

    public Sprite GetCurrencySprite(CurrencyEnum currency)
    {
        return CurrencySprites.First(x => x.type == currency).sprite;
    }

    #endregion Currency

    #region Skills

    public Sprite GetSkillSprite(string skillName)
    {
        return SkillSprites.First(x => x.name == skillName);
    }

    public List<Sprite> GetUniqueSkillSprites(int count)
    {
        List<Sprite> sprite = new List<Sprite>();

        for (int i = 0; i < count; i++)
        {
            sprite.Add(SkillSprites.First(x => !sprite.Contains(x)));
        }

        return sprite;
    }

    #endregion Skills
}

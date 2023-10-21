using System.Collections;
using System.Collections.Generic;
using WeaponRelated;

public class WeaponData
{
    public string weaponId = "";
    public bool isEquipped = false;

    public WeaponRankEnum weaponRank;
    public WeaponTypeEnum weaponType;

    public float damage_physical = 0;
    public float damage_magic = 0;

    public float durability = 0;

    public float spirit_Experience = 0;

    public List<string> sacrificedWeapons = new List<string>();
}

using System.Collections;
using System.Collections.Generic;
using WeaponRelated;

public class WeaponData
{
    public WeaponData()
    {

    }
    public WeaponData(WeaponData weaponData) 
    {
        weaponId = weaponData.weaponId;
        isEquipped = weaponData.isEquipped;
        weaponRank = weaponData.weaponRank;
        weaponType = weaponData.weaponType;

        damage_physical = weaponData.damage_physical;
        damage_magic = weaponData.damage_magic;

        durability = weaponData.durability;
        spirit_Experience = weaponData.spirit_Experience;

        behaviorSkillSlotCount = weaponData.behaviorSkillSlotCount;
        attributeSlotCount  = weaponData.attributeSlotCount;

        sacrificedWeapons = new List<string>(weaponData.sacrificedWeapons);
    }

    public string weaponId = "";
    public bool isEquipped = false;

    public WeaponRankEnum weaponRank;
    public WeaponTypeEnum weaponType;

    public float damage_physical = 0;
    public float damage_magic = 0;

    public float durability = 0;

    public float spirit_Experience = 0;

    public int behaviorSkillSlotCount = 0;
    public int attributeSlotCount = 0;

    public List<string> sacrificedWeapons = new List<string>();
}

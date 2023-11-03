using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WeaponRelated;


[DataContract]
public class WeaponData
{
    #region Constructors

    public WeaponData()
    {

    }
    public WeaponData(WeaponData weaponData) 
    {
        weaponId = weaponData.weaponId;
        isEquipped = weaponData.isEquipped;
        weaponRank = weaponData.weaponRank;
        weaponType = weaponData.weaponType;

        weaponHealth = weaponData.weaponHealth;

        skills = new List<SkillData>(weaponData.skills);

        damage_physical = weaponData.damage_physical;
        damage_magic = weaponData.damage_magic;

        durability = weaponData.durability;
        spirit_Experience = weaponData.spirit_Experience;

        behaviorSkillSlotCount = weaponData.behaviorSkillSlotCount;
        attributeSlotCount  = weaponData.attributeSlotCount;

        sacrificedWeapons = new List<string>(weaponData.sacrificedWeapons);
    }

    #endregion Constructors

    [DataMember]
    public string weaponId = "";

    [DataMember]
    public bool isEquipped = false;

    [DataMember]
    public WeaponRankEnum weaponRank;

    [DataMember]
    public WeaponTypeEnum weaponType;

    [DataMember]
    public float weaponHealth = 0;

    [DataMember]
    public float damage_physical = 0;

    [DataMember]
    public float damage_magic = 0;

    [DataMember]
    public float durability = 0;

    [DataMember]
    public float spirit_Experience = 0;

    [DataMember]
    public int behaviorSkillSlotCount = 0;

    [DataMember]
    public int attributeSlotCount = 0;

    [DataMember]
    public List<SkillData> skills = new List<SkillData>();

    [DataMember]
    public List<PillAttributeData> attributes = new List<PillAttributeData>();

    [DataMember]
    public SkillData skillSlotInQuestion = null;

    [DataMember]
    public SkillData skillPurchased = null;
    
    [DataMember]
    public SkillData lastUpgradedSkill = null;

    [DataMember]
    public List<string> sacrificedWeapons = new List<string>();
}

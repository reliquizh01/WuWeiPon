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

        weapon_Health = weaponData.weapon_Health;
        spirit_Experience = weaponData.spirit_Experience;

        skills = new List<SkillData>(weaponData.skills);

        damage_Physical = weaponData.damage_Physical;
        damage_Magic = weaponData.damage_Magic;

        critChance = weaponData.critChance;
        critPercentDamage = weaponData.critPercentDamage;

        cooldown_Reduction = weaponData.cooldown_Reduction;
        
        armor_Penetration = weaponData.armor_Penetration;
        armor_Physical = weaponData.armor_Physical;
        armor_Magic = weaponData.armor_Magic;

        status_Resistance = weaponData.status_Resistance;
        poison_Resistance = weaponData.poison_Resistance;

        monster_Damage = weaponData.monster_Damage;
        luck = weaponData.luck;
        evasion = weaponData.evasion;
        spin_Speed = weaponData.spin_Speed;

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

    #region Stats

    [DataMember]
    public float weapon_Health = 0;

    [DataMember]
    public float damage_Physical = 0;

    [DataMember]
    public float damage_Magic = 0;

    [DataMember]
    public float cooldown_Reduction = 0;

    [DataMember]
    public float armor_Penetration = 0;

    [DataMember]
    public float armor_Physical = 0;
    
    [DataMember]
    public float armor_Magic = 0;

    [DataMember]
    public float status_Resistance = 0;

    [DataMember]
    public float poison_Resistance = 0;
    
    [DataMember]
    public float monster_Damage = 0;

    [DataMember]
    public float luck = 0;
    
    [DataMember]
    public float evasion = 0;

    [DataMember]
    public float spin_Speed = 0;

    [DataMember]
    public float critChance = 0;

    [DataMember]
    public float critPercentDamage = 0;

    #endregion Stats

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

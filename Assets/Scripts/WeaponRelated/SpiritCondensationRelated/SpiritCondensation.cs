using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

[DataContract]
public class SpiritCondensation
{

    #region Constructor

    public SpiritCondensation(WeaponData weaponData, float amount)
    {
        currentCondensedWeapon = weaponData;
        spiritualAmount = amount;
    }

    public SpiritCondensation(SpiritCondensation spiritCondensation)
    {
        spiritualAmount = spiritCondensation.spiritualAmount;
        currentCondensedWeapon = spiritCondensation.currentCondensedWeapon;
        potentialSpiritEnhancements = new List<SpiritEnhancement>(spiritCondensation.potentialSpiritEnhancements);
    }

    #endregion Constructor

    [DataMember]
    public WeaponData currentCondensedWeapon = null;

    [DataMember]
    public float spiritualAmount;

    [DataMember]
    public List<SpiritEnhancement> potentialSpiritEnhancements = new List<SpiritEnhancement>();

    public void GenerateCondensation()
    {
        int allottedSpiritualEnhancement = 0;

        if(spiritualAmount == 1000.0f)
        {
            allottedSpiritualEnhancement = 12;
        }
        else
        {
            allottedSpiritualEnhancement = Mathf.RoundToInt(spiritualAmount / 120.0f);
        }

        int maxStatCount = Enum.GetValues(typeof(WeaponStatEnum)).Length;

        for(int i = 0; i < allottedSpiritualEnhancement; i++)
        {
            SpiritEnhancement tmp = new SpiritEnhancement();
            tmp.WeaponStat = (WeaponStatEnum)UnityEngine.Random.Range(0, maxStatCount);

            potentialSpiritEnhancements.Add(tmp);

            int thisStat = potentialSpiritEnhancements.Count - 1;

            switch (tmp.WeaponStat)
            {
                case WeaponStatEnum.weapon_Health:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 10.0f;
                    break;
                case WeaponStatEnum.damage_Physical:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.25f;
                    break;
                case WeaponStatEnum.damage_Magic:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.1f;
                    break;
                case WeaponStatEnum.cooldown_Reduction:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.001f;
                    break;
                case WeaponStatEnum.armor_Penetration:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.01f;
                    break;
                case WeaponStatEnum.armor_Physical:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.05f;
                    break;
                case WeaponStatEnum.armor_Magic:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.05f;
                    break;
                case WeaponStatEnum.status_Resistance:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.1f;
                    break;
                case WeaponStatEnum.poison_Resistance:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.1f;
                    break;
                case WeaponStatEnum.monster_Damage:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 10.0f;
                    break;
                case WeaponStatEnum.luck:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.25f;
                    break;
                case WeaponStatEnum.evasion:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.01f;
                    break;
                case WeaponStatEnum.spin_Speed:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 1.0f;
                    break;
                case WeaponStatEnum.critChance:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.02f;
                    break;
                case WeaponStatEnum.critPercentDamage:
                    potentialSpiritEnhancements[thisStat].EnhancementAmount += 0.1f;
                    break;

                default:
                    break;
            }
        }
    }
}

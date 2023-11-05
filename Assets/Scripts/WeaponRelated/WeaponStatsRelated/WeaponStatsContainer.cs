using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class WeaponStatsContainer : MonoBehaviour
{
    public BaseUserInterfaceNotificationBehavior notification;
    public WeaponStatEnum weaponStat;

    public TextMeshProUGUI statText;
    public TextMeshProUGUI amountText;
    public float currentAmount;
    private Timer timer;

    public void SetAmountFromWeaponData(WeaponData weaponData)
    {
        statText.text = UniformityConverter.StatEnumToStatName(weaponStat);

        switch (weaponStat)
        {
            case WeaponStatEnum.weapon_Health:
                currentAmount = weaponData.weapon_Health;
                break;
            case WeaponStatEnum.damage_Physical:
                currentAmount = weaponData.damage_Physical;
                break;
            case WeaponStatEnum.damage_Magic:
                currentAmount = weaponData.damage_Magic;
                break;
            case WeaponStatEnum.cooldown_Reduction:
                currentAmount = weaponData.cooldown_Reduction;
                break;
            case WeaponStatEnum.armor_Penetration:
                currentAmount = weaponData.armor_Penetration;
                break;
            case WeaponStatEnum.armor_Physical:
                currentAmount = weaponData.armor_Physical;
                break;
            case WeaponStatEnum.armor_Magic:
                currentAmount = weaponData.armor_Magic;
                break;
            case WeaponStatEnum.status_Resistance:
                currentAmount = weaponData.status_Resistance;
                break;
            case WeaponStatEnum.poison_Resistance:
                currentAmount = weaponData.poison_Resistance;
                break;
            case WeaponStatEnum.monster_Damage:
                currentAmount = weaponData.monster_Damage;
                break;
            case WeaponStatEnum.luck:
                currentAmount = weaponData.luck;
                break;
            case WeaponStatEnum.evasion:
                currentAmount = weaponData.evasion;
                break;
            case WeaponStatEnum.spin_Speed:
                currentAmount = weaponData.spin_Speed;
                break;
            case WeaponStatEnum.critChance:
                currentAmount = weaponData.critChance;
                break;
            case WeaponStatEnum.critPercentDamage:
                currentAmount = weaponData.critPercentDamage;
                break;
            
            default:
                currentAmount = -9999; // Default value for unknown stats
                break;
        }

        amountText.text = UniformityConverter.StatValueToStatString(weaponStat, currentAmount);
    }

}

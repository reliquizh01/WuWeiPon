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
    public float IncrementToThisAmount;
    private float currentAmount = 0;
    private Timer timer;

    public void SetAmountFromWeaponData(WeaponData weaponData)
    {
        switch (weaponStat)
        {
            case WeaponStatEnum.weapon_Health:
                statText.text = "Total HP";
                IncrementToThisAmount = weaponData.weapon_Health;
                break;
            case WeaponStatEnum.damage_Physical:
                statText.text = "Phys. DMG";
                IncrementToThisAmount = weaponData.damage_Physical;
                break;
            case WeaponStatEnum.damage_Magic:
                statText.text = "Magic DMG";
                IncrementToThisAmount = weaponData.damage_Magic;
                break;
            case WeaponStatEnum.cooldown_Reduction:
                statText.text = "CD Reduc.";
                IncrementToThisAmount = weaponData.cooldown_Reduction;
                break;
            case WeaponStatEnum.armor_Penetration:
                statText.text = "Armor Pen.";
                IncrementToThisAmount = weaponData.armor_Penetration;
                break;
            case WeaponStatEnum.armor_Physical:
                statText.text = "Armor Phys.";
                IncrementToThisAmount = weaponData.armor_Physical;
                break;
            case WeaponStatEnum.armor_Magic:
                statText.text = "Armor Magic";
                IncrementToThisAmount = weaponData.armor_Magic;
                break;
            case WeaponStatEnum.status_Resistance:
                statText.text = "Status Res.";
                IncrementToThisAmount = weaponData.status_Resistance;
                break;
            case WeaponStatEnum.poison_Resistance:
                statText.text = "Poison Res.";
                IncrementToThisAmount = weaponData.poison_Resistance;
                break;
            case WeaponStatEnum.monster_Damage:
                statText.text = "Monster DMG";
                IncrementToThisAmount = weaponData.monster_Damage;
                break;
            case WeaponStatEnum.luck:
                statText.text = "Luck";
                IncrementToThisAmount = weaponData.luck;
                break;
            case WeaponStatEnum.evasion:
                statText.text = "Evasion";
                IncrementToThisAmount = weaponData.evasion;
                break;
            case WeaponStatEnum.spin_Speed:
                statText.text = "Spin SPD";
                IncrementToThisAmount = weaponData.spin_Speed;
                break;
            case WeaponStatEnum.critChance:
                statText.text = "Crit. Rate";
                IncrementToThisAmount = weaponData.critChance;
                break;
            case WeaponStatEnum.critPercentDamage:
                statText.text = "Crit. DMG %";
                IncrementToThisAmount = weaponData.critPercentDamage;
                break;
            
            default:
                IncrementToThisAmount = -9999; // Default value for unknown stats
                break;
        }

        StartIncrementalUpdate();
    }

    public void StartIncrementalUpdate()
    {
        if(IncrementToThisAmount > 0 && currentAmount != IncrementToThisAmount)
        {
            // Calculate the incremental value to add during each timer tick.
            float incrementalValue = IncrementToThisAmount / 100.0f;

            // Create a timer that runs every 0.01 seconds.
            timer = new Timer(IncrementAmount, incrementalValue, 0, 10);
            notification.Play();
        }
        else
        {
            currentAmount = IncrementToThisAmount;
            amountText.text = currentAmount.ToString("N2");
        }
    }

    private void IncrementAmount(object state)
    {
        float incrementalValue = (float)state;

        if (currentAmount < IncrementToThisAmount)
        {
            currentAmount += incrementalValue;

            if (currentAmount > IncrementToThisAmount)
            {
                currentAmount = IncrementToThisAmount;
            }

            // You can use or display the currentAmount as needed.
            amountText.text = currentAmount.ToString("N2");
        }
        else
        {
            // If the currentAmount has reached the target, stop the timer.
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
        }
    }
}

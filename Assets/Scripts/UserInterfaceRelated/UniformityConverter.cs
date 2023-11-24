using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class UniformityConverter
{
    #region Skill Names

    public static string SkillEnumToSkillName(SkillEnum skill)
    {
        string skillName = "";

        switch (skill)
        {
            case SkillEnum.FirstBloodSpill:
                skillName = "First Blood Spill";
                break;
            case SkillEnum.CornerBoost:
                skillName = "Corner Boost";
                break;
            case SkillEnum.LifeSiphon:
                skillName = "Life Siphon";
                break;
            default:
                break;
        }

        return skillName;
    }

    public static string SkillStringToSkillName(string skillName)
    {
        var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        skillName = r.Replace(skillName, " ");

        return skillName;
    }

    public static SkillEnum SkillStringToEnum(string skillName)
    {
        SkillEnum skill;
        Enum.TryParse(skillName, out skill);

        return skill;
    }

    #endregion Skill Names

    #region Skill Stat Description and Name

    public static string SkillNameToStatDescription(SkillEnum skill, SkillData skillData)
    {
        string description = "";

        switch (skill)
        {
            case SkillEnum.FirstBloodSpill:
                float amount = 0;
                amount += (float)skillData.skillValues[SkillVariableNames.ADD_DAMAGE_PERCENTAGE];
                SkillProgressionBonus.AmplifyDamagePercentage(skillData, ref amount);
                
                description = string.Format(
                    SkillVariableNames.FIRSTBLOODSPILL_DESCRIPTION,
                    amount,
                    skillData.skillValues[SkillVariableNames.ADD_MAXIMUM_USAGE]);
                break;
            case SkillEnum.CornerBoost:
                float burstAmount = 0;
                burstAmount += (float)skillData.skillValues[SkillVariableNames.ADD_BURST_SPEED_FORCE];
                SkillProgressionBonus.AmplifyMovementBurst(skillData, ref burstAmount);

                description = string.Format(
                    SkillVariableNames.CORNERBOOST_DESCRIPTION,
                    burstAmount,
                    skillData.skillValues[SkillVariableNames.ADD_COOLDOWN]);
                break;
            case SkillEnum.LifeSiphon:
                float lifestealPercentage = 0;
                lifestealPercentage += (float)skillData.skillValues[SkillVariableNames.ADD_HEALING_PERCENTAGE];
                SkillProgressionBonus.AmplifyHealingPercentage(skillData, ref lifestealPercentage);

                description = string.Format(SkillVariableNames.LIFESIPHON_DESCRIPTION,
                    lifestealPercentage);
                break;
            default:
                break;
        }

        return description;
    }

    internal static string StatEnumToStatName(WeaponStatEnum weaponStatEnum)
    {
        string name = "";

        switch (weaponStatEnum)
        {
            case WeaponStatEnum.weapon_Health:
                name = "HP";
                break;
            case WeaponStatEnum.damage_Physical:
                name = "P.ATK";
                // Handle physical damage logic here
                break;
            case WeaponStatEnum.damage_Magic:
                name = "M.DMG";
                // Handle magic damage logic here
                break;
            case WeaponStatEnum.cooldown_Reduction:
                name = "CD";
                // Handle cooldown reduction logic here
                break;
            case WeaponStatEnum.armor_Penetration:
                name = "A.PEN.";
                // Handle armor penetration logic here
                break;
            case WeaponStatEnum.armor_Physical:
                name = "P.Armor";
                // Handle physical armor logic here
                break;
            case WeaponStatEnum.armor_Magic:
                name = "M.Armor";
                // Handle magic armor logic here
                break;
            case WeaponStatEnum.status_Resistance:
                name = "Status Res.";
                // Handle status resistance logic here
                break;
            case WeaponStatEnum.poison_Resistance:
                name = "Poison Res.";
                // Handle poison resistance logic here
                break;
            case WeaponStatEnum.monster_Damage:
                name = "Monster DMG.";
                // Handle monster damage logic here
                break;
            case WeaponStatEnum.luck:
                name = "Luck";
                // Handle luck logic here
                break;
            case WeaponStatEnum.evasion:
                name = "Evasion";
                // Handle evasion logic here
                break;
            case WeaponStatEnum.spin_Speed:
                name = "Speed";
                // Handle spin speed logic here
                break;
            case WeaponStatEnum.critChance:
                name = "Crit. Rate";
                // Handle critical chance logic here
                break;
            case WeaponStatEnum.critPercentDamage:
                name = "Crit.DMG %";
                // Handle critical percent damage logic here
                break;
            default:
                // Handle unknown weapon stat logic here
                break;
        }

        return name;
    }

    internal static string StatValueToStatString(WeaponStatEnum weaponStatEnum, float value)
    {
        string convertedString = "";

        switch (weaponStatEnum)
        {
            case WeaponStatEnum.weapon_Health:
                convertedString = value.ToString("N2");
                break;
            case WeaponStatEnum.damage_Physical:
                convertedString = value.ToString("N2");
                // Handle physical damage logic here
                break;
            case WeaponStatEnum.damage_Magic:
                convertedString = value.ToString("N2");
                // Handle magic damage logic here
                break;
            case WeaponStatEnum.cooldown_Reduction:
                convertedString = value.ToString("N2") + "s";
                // Handle cooldown reduction logic here
                break;
            case WeaponStatEnum.armor_Penetration:
            case WeaponStatEnum.armor_Physical:
            case WeaponStatEnum.armor_Magic:
            case WeaponStatEnum.status_Resistance:
            case WeaponStatEnum.poison_Resistance:
            case WeaponStatEnum.critChance:
            case WeaponStatEnum.critPercentDamage:
                if(value < 10.0f)
                {
                    convertedString = value.ToString("N2") + "%";
                }
                else
                {
                    convertedString = value.ToString("N1") + "%";
                }
                break;
            case WeaponStatEnum.monster_Damage:
                convertedString = value.ToString("N2");
                // Handle monster damage logic here
                break;
            case WeaponStatEnum.luck:
                convertedString = value.ToString("N3");
                // Handle luck logic here
                break;
            case WeaponStatEnum.evasion:
                convertedString = value.ToString("N2");
                // Handle evasion logic here
                break;
            case WeaponStatEnum.spin_Speed:
                convertedString = value.ToString("N2");
                // Handle spin speed logic here
                break;
            default:
                // Handle unknown weapon stat logic here
                break;
        }

        return convertedString;
    }

    internal static UnityEngine.Color SkillRankToColor(SkillRankEnum skillRank)
    {
        UnityEngine.Color color = UnityEngine.Color.white;

        switch (skillRank)
        {
            case SkillRankEnum.ordinary:
                ColorUtility.TryParseHtmlString("#0BFF00", out  color);
                break;
            case SkillRankEnum.rare:
                ColorUtility.TryParseHtmlString("#FFBD00", out  color);
                break;
            case SkillRankEnum.transcendant:
                ColorUtility.TryParseHtmlString("#FFE900", out  color);
                break;
            case SkillRankEnum.ancient:
                ColorUtility.TryParseHtmlString("#FF9900", out  color);
                break;
            case SkillRankEnum.divine:
                ColorUtility.TryParseHtmlString("#E700FF", out  color);
                break;
            case SkillRankEnum.deity:
                ColorUtility.TryParseHtmlString("#FF1F00", out  color);
                break;
            default:
                break;
        }

        return color;
    }

    internal static string SkillRankToString(SkillRankEnum skillRank)
    {
        string  convertedString = "";

        switch (skillRank)
        {
            case SkillRankEnum.ordinary:
                convertedString = "Ordinary";
                break;
            case SkillRankEnum.rare:
                convertedString = "Rare";
                break;
            case SkillRankEnum.transcendant:
                convertedString = "Trancendant";
                break;
            case SkillRankEnum.ancient:
                convertedString = "Ancient";
                break;
            case SkillRankEnum.divine:
                convertedString = "Divine";
                break;
            case SkillRankEnum.deity:
                convertedString = "Deity";
                break;
            default:
                break;
        }

        return convertedString;
    }

    internal static string RecordStatsResultToString(RecordStatsEnum statEnum)
    {
        string convertedString = "";

        switch (statEnum)
        {
            case RecordStatsEnum.DamageTaken:
                convertedString = "DMG TAKEN";
                break;
            case RecordStatsEnum.HighestOneHitDamage:
                convertedString = "1-HIT DMG";
                break;
            case RecordStatsEnum.TotalDamage:
                convertedString = "TOTAL DMG";
                break;
            default:
                break;
        }

        return convertedString;
    }

    #endregion Skill Stat Description and Name
}
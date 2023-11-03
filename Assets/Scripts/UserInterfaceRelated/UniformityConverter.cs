using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

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

    #region Skill Stat Description

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

    #endregion Skill Stat Description
}
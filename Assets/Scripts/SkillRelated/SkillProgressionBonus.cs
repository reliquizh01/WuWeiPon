

using System;

public static class SkillProgressionBonus
{
    public static void AmplifyMovementBurst(SkillData skillData, ref float currentMovementBurst)
    {
        SkillEnum skill;
        Enum.TryParse(skillData.skillName, out skill);

        switch (skill)
        {
            case SkillEnum.CornerBoost:
                float addedBoost = 2.0f * skillData.skillLevel;
                currentMovementBurst += addedBoost;
                break;
            default:
                break;
        }
    }


    public static void AmplifyDamagePercentage(SkillData skillData, ref float damagePercentage)
    {
        SkillEnum skill;
        Enum.TryParse(skillData.skillName, out skill);

        switch (skill)
        {
            case SkillEnum.FirstBloodSpill:
                float addedPercentage = 3.0f * skillData.skillLevel;
                damagePercentage += addedPercentage;
                break;
            default:
                break;
        }
    }
    
    public static void AmplifyHealingPercentage(SkillData skillData, ref float healingPercentage)
    {
        SkillEnum skill;
        Enum.TryParse(skillData.skillName, out skill);

        switch (skill)
        {
            case SkillEnum.LifeSiphon:
                float addedPercentage = 0.02f * skillData.skillLevel;
                healingPercentage += addedPercentage;
                break;
            default:
                break;
        }
    }
}
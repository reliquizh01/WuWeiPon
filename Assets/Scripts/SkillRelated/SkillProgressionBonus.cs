

using System;

public static class SkillProgressionBonus
{
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
}
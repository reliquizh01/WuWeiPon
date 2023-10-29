

using System;

namespace SkillRelated
{
    public static class SkillGenerator
    {
        public static SkillData GenerateSkill()
        {
            SkillData skillData = new SkillData();

            SkillEnum skillRandom = (SkillEnum)UnityEngine.Random.Range(0, Enum.GetNames(typeof(SkillEnum)).Length);

            switch (skillRandom)
            {
                case SkillEnum.FirstBloodSpill:
                    skillData.skillName = SkillEnum.FirstBloodSpill.ToString();
                    skillData.description = SkillVariableNames.FIRSTBLOODSPILL_DESCRIPTION;
                    skillData.skillIconFileName = SkillEnum.FirstBloodSpill.ToString() + "_Icon";
                    skillData.skillType = SkillTypeEnum.Damage;

                    skillData.isSkillConditionOnHit = true;

                    skillData.skillValues.Add(SkillVariableNames.SET_TARGET_TO_HILTS, true);
                    skillData.skillValues.Add(SkillVariableNames.SET_MAXIMUM_USAGE, true);
                    skillData.skillValues.Add(SkillVariableNames.ADD_MAXIMUM_USAGE, 1);
                    skillData.skillValues.Add(SkillVariableNames.ADD_DAMAGE_PERCENTAGE, 50.0f);
                    break;

                case SkillEnum.CornerBoost:
                    skillData.skillName = SkillEnum.CornerBoost.ToString();
                    skillData.description = SkillVariableNames.CORNERBOOST_DESCRIPTION;
                    skillData.skillIconFileName = SkillEnum.CornerBoost.ToString() + "_Icon";
                    skillData.skillType = SkillTypeEnum.Movement;
                
                    skillData.isSkillConditionOnHit = true;

                    AddCooldown(skillData, 30.0f);
                    skillData.skillValues.Add(SkillVariableNames.SET_TARGET_TO_WALLS, true);
                    skillData.skillValues.Add(SkillVariableNames.ADD_BURST_SPEED_FORCE, 150.0f);
                    break;
                default:
                    break;
            }

            return skillData;
        }

        private static void AddCooldown(SkillData skillData, float cooldown)
        {
            skillData.skillValues.Add(SkillVariableNames.ADD_COOLDOWN, cooldown);
        }
    }

}
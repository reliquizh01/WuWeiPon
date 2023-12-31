

using System;

namespace SkillRelated
{
    public static class SkillGenerator
    {
        internal static SkillData GenerateSkill()
        {
            SkillData skillData = new SkillData();

            SkillEnum skillRandom = (SkillEnum)UnityEngine.Random.Range(0, Enum.GetNames(typeof(SkillEnum)).Length);

            switch (skillRandom)
            {
                case SkillEnum.FirstBloodSpill:
                    skillData.skillName = SkillEnum.FirstBloodSpill.ToString();
                    skillData.description = SkillVariableNames.FIRSTBLOODSPILL_DESCRIPTION;
                    skillData.skillIconFileName = SkillEnum.FirstBloodSpill.ToString() + SkillVariableNames._Icon;
                    skillData.skillType = SkillTypeEnum.Damage;
                    skillData.skillRank = SkillRankEnum.ordinary;

                    skillData.isSkillConditionOnHit = true;

                    skillData.skillValues.Add(SkillVariableNames.SET_DETECTION_TO_BLADE_ONLY, true);
                    skillData.skillValues.Add(SkillVariableNames.SET_TARGET_TO_HILTS, true);
                    skillData.skillValues.Add(SkillVariableNames.SET_MAXIMUM_USAGE, true);
                    skillData.skillValues.Add(SkillVariableNames.ADD_MAXIMUM_USAGE, 1);
                    skillData.skillValues.Add(SkillVariableNames.ADD_DAMAGE_PERCENTAGE, 50.0f);
                    break;

                case SkillEnum.CornerBoost:
                    skillData.skillName = SkillEnum.CornerBoost.ToString();
                    skillData.description = SkillVariableNames.CORNERBOOST_DESCRIPTION;
                    skillData.skillIconFileName = SkillEnum.CornerBoost.ToString() + SkillVariableNames._Icon;
                    skillData.skillType = SkillTypeEnum.Movement;
                    skillData.skillRank = SkillRankEnum.ordinary;

                    skillData.isSkillConditionOnHit = true;
                    
                    AddCooldown(skillData, 30.0f);
                    
                    skillData.skillValues.Add(SkillVariableNames.SET_DETECTION_TO_HILT_ONLY, true);
                    skillData.skillValues.Add(SkillVariableNames.SET_TARGET_TO_WALLS, true);

                    skillData.skillValues.Add(SkillVariableNames.ADD_BURST_SPEED_FORCE, 25.0f);
                    break;
                case SkillEnum.LifeSiphon:
                    skillData.skillName = SkillEnum.LifeSiphon.ToString();
                    skillData.description = SkillVariableNames.LIFESIPHON_DESCRIPTION;
                    skillData.skillRank = SkillRankEnum.rare;

                    skillData.skillIconFileName = SkillEnum.LifeSiphon.ToString() + SkillVariableNames._Icon;
                    skillData.skillType = SkillTypeEnum.Heal;

                    skillData.skillValues.Add(SkillVariableNames.SET_DETECTION_TO_BLADE_ONLY, true);
                    skillData.skillValues.Add(SkillVariableNames.SET_TARGET_TO_HILTS, true);

                    skillData.skillValues.Add(SkillVariableNames.SET_HEALING_ONCE, true);
                    skillData.skillValues.Add(SkillVariableNames.ADD_HEALING_PERCENTAGE, 0.1f);
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
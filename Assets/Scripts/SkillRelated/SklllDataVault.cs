
namespace SKillRelated
{
    public static class SklllDataVault
    {
        public static SkillData GenerateSkill(SkillEnum skill)
        {
            SkillData skillData = new SkillData();

            switch (skill)
            {
                case SkillEnum.FirstBloodSpill:
                    SetupFirstBloodSpill(skillData);
                    break;
            }

            return skillData;
        }

        private static void SetupFirstBloodSpill(SkillData skillData)
        {
            skillData.skillType = SkillTypeEnum.Damage;
            skillData.skillValues.Add(SkillVariableNames.ADD_DAMAGE_PERCENTAGE, 50.0f);
            skillData.skillValues.Add(SkillVariableNames.ADD_MAXIMUM_USAGE, 1.0f);
        }
    }
}
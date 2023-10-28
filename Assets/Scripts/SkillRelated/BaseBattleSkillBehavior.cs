public class BaseBattleSkillBehavior
{
    public bool hasCooldown = false;
    public float cooldown = 0.0f;

    public bool hasMaxUsage = false;
    public int maxUseCount = 0;
    public int currentUseCount = 0;

    public virtual void InitializeSkill(SkillData skillData)
    {
        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_COOLDOWN))
        {
            hasCooldown = true;
            cooldown = skillData.skillValues[SkillVariableNames.ADD_COOLDOWN];
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_MAXIMUM_USAGE))
        {
            hasMaxUsage = true;
            maxUseCount = (int)skillData.skillValues[SkillVariableNames.ADD_MAXIMUM_USAGE];
        }
    }
}
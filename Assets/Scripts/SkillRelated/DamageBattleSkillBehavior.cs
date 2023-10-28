public class DamageBattleSkillBehavior : BaseBattleSkillBehavior
{
    public bool hasDamageBonus = false;
    public float addDamagePercentage = 0.0f;
    public float addDamageValue = 0.0f;

    public override void InitializeSkill(SkillData skillData)
    {
        base.InitializeSkill(skillData);

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_DAMAGE_PERCENTAGE))
        {
            hasDamageBonus = true;
            addDamagePercentage = skillData.skillValues[SkillVariableNames.ADD_DAMAGE_PERCENTAGE];
        }
    }
}
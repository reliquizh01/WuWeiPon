

using WeaponRelated;

public class HealBattleSkillBehavior : BaseBattleSkillBehavior
{
    public bool HealOnceOnCall = false;
    public float healPercentage = 0.0f;
    public float oneTimeHealAmount = 0.0f;

    public bool HealOvertime= false;
    public float healOvertime = 0.0f;
    public float healPercentageOvertime = 0.0f;

    public override void InitializeSkill(SkillData skillData)
    {
        base.InitializeSkill(skillData);

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_HEALING_ONCE))
        {
            HealOnceOnCall = true;
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_HEALING_PERCENTAGE))
        {
            healPercentage = (float)skillData.skillValues[SkillVariableNames.ADD_HEALING_PERCENTAGE];
            SkillProgressionBonus.AmplifyHealingPercentage(skillData,ref healPercentage);
        }
    }

    public void HealWeaponBasedOnDamagePercentage(ref float healToInflict, ref float damageToInflict)
    {
        healToInflict += (damageToInflict * healPercentage);
    }
}
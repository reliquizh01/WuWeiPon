using UnityEngine;
using WeaponRelated;

public class DamageBattleSkillBehavior : BaseBattleSkillBehavior
{
    public bool hasDamageBonusByPercent = false;
    public float addDamagePercentage = 0.0f;

    public bool hasDamageBonusByValue = false;
    public float addDamageValue = 0.0f;

    public override void InitializeSkill(SkillData skillData)
    {
        base.InitializeSkill(skillData);

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_DAMAGE_PERCENTAGE))
        {
            hasDamageBonusByPercent = true;
            addDamagePercentage = (float)skillData.skillValues[SkillVariableNames.ADD_DAMAGE_PERCENTAGE];
            SkillProgressionBonus.AmplifyDamagePercentage(skillData, ref addDamagePercentage);
        }
    }

    /// <summary>
    /// The method that the blade will call to modify the damage right before they send it to the hilt who took it.
    /// </summary>
    /// <param name="referencedAmountToModify">referenced damage that will be modified</param>
    public void EnhanceDamageToBeInflicted(ref float referencedAmountToModify)
    {
        if(!isMaxUsageReached())
        {
            if (hasDamageBonusByPercent)
            {
                float addedRealValue = 0;
                addedRealValue += referencedAmountToModify * (addDamagePercentage / 100.0f);

                referencedAmountToModify += addedRealValue;
            }

            if (hasDamageBonusByValue)
            {
                referencedAmountToModify += addDamageValue;
            }

            IncrementMaxUsage();
        }
    }
}
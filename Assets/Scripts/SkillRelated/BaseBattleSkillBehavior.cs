using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using WeaponRelated;

public class BaseBattleSkillBehavior
{
    public bool hasCooldown = false;
    public float cooldown = 0.0f;

    public bool hasMaxUsage = false;
    public int maxUseCount = 0;
    public int currentUseCount = 0;

    public List<SkillTargetEnum> skillTargetEnums = new List<SkillTargetEnum>();

    public virtual void InitializeSkill(SkillData skillData)
    {
        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_COOLDOWN))
        {
            hasCooldown = true;
            cooldown = (float)skillData.skillValues[SkillVariableNames.ADD_COOLDOWN];
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_MAXIMUM_USAGE))
        {
            hasMaxUsage = true;
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_MAXIMUM_USAGE))
        {
            maxUseCount = (int)skillData.skillValues[SkillVariableNames.ADD_MAXIMUM_USAGE];
        }
    }

    public virtual SkillTargetEnum CheckSkillConditionOnHit(Collider2D hitObject)
    {
        SkillTargetEnum hit = SkillTargetEnum.None;

        foreach (SkillTargetEnum item in skillTargetEnums)
        {
            switch (item)
            {
                case SkillTargetEnum.Hilt:
                    if (hitObject.GetComponent<WeaponHiltBehavior>() != null) hit = SkillTargetEnum.Hilt;
                    break;
                case SkillTargetEnum.Walls:
                    int layer = LayerMask.NameToLayer("Walls");
                    if (hitObject.gameObject.layer == layer) hit = SkillTargetEnum.Walls;
                    break;
                case SkillTargetEnum.Blade:
                    if (hitObject.GetComponent<WeaponBladeBehavior>() != null) hit = SkillTargetEnum.Blade;
                    break;
                default:
                    break;
            }
        }

        return hit;
    }
}
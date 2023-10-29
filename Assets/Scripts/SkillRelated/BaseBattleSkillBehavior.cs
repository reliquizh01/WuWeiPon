using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using WeaponRelated;

public class BaseBattleSkillBehavior
{
    public bool SkillIsReady 
    { 
        get
        {
            if (hasCooldown && !hasMaxUsage)
            {
                return (currentCooldown > cooldown);
            }
            else if (hasMaxUsage && !hasCooldown)
            {
                return (maxUseCount > currentUseCount);
            }
            else if(hasMaxUsage && hasCooldown)
            {
                return ((maxUseCount > currentCooldown) && (cooldown > currentCooldown));
            }
            else
            {
                return true;
            }
        }
    }

    public bool hasCooldown = false;
    public float cooldown = 0.0f;
    public float currentCooldown = 0.0f;

    public bool hasMaxUsage = false;
    public int maxUseCount = 0;
    public int currentUseCount = 0;

    public List<SkillTargetEnum> skillTargetEnums = new List<SkillTargetEnum>();

    public virtual void Update(float deltaTime)
    {
        if (hasCooldown)
        {
            if(currentCooldown < cooldown)
            {
                currentCooldown += deltaTime;
            }
        }
    }

    public virtual void InitializeSkill(SkillData skillData)
    {
        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_COOLDOWN))
        {
            hasCooldown = true;
            cooldown = (float)skillData.skillValues[SkillVariableNames.ADD_COOLDOWN];
            currentCooldown = 0.0f;
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_MAXIMUM_USAGE))
        {
            hasMaxUsage = true;
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.ADD_MAXIMUM_USAGE))
        {
            maxUseCount = (int)skillData.skillValues[SkillVariableNames.ADD_MAXIMUM_USAGE];
            currentUseCount = 0;
        }

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_TARGET_TO_HILTS))
        {
            skillTargetEnums.Add(SkillTargetEnum.Hilt);
        }
        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_TARGET_TO_WALLS))
        {
            skillTargetEnums.Add(SkillTargetEnum.Walls);
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
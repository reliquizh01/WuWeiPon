using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using WeaponRelated;

public class BaseBattleSkillBehavior
{
    internal string skillName; 

    public bool hasCooldown = false;
    public bool cooldownStarted = false;
    public float cooldown = 0.0f;
    public float currentCooldown = 0.0f;

    public bool hasMaxUsage = false;
    public int maxUseCount = 0;
    public int currentUseCount = 0;

    public List<SkillTargetEnum> skillTargetEnums = new List<SkillTargetEnum>();

    public List<Action> onMaxUsageReached = new List<Action>();

    public List<Action> onCooldownUpdate = new List<Action>();
    public List<Action> onCooldownFinish = new List<Action>();

    public virtual void Update(float deltaTime)
    {
        if (hasCooldown && cooldownStarted)
        {
            if(currentCooldown < cooldown)
            {
                currentCooldown += deltaTime;
            }
            else
            {
                currentCooldown = 0.0f;
                cooldownStarted = false;

                if(onCooldownFinish.Count > 0)
                {
                    onCooldownFinish.ForEach(x => x.Invoke());
                }
            }
        }
    }

    public virtual void InitializeSkill(SkillData skillData)
    {
        skillName = skillData.skillName;

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
    public virtual SkillTargetEnum CheckSkillConditionOnHit(object hitObject)
    {
        SkillTargetEnum hit = SkillTargetEnum.None;

        foreach (SkillTargetEnum item in skillTargetEnums)
        {
            switch (item)
            {
                case SkillTargetEnum.Hilt:
                    WeaponHiltBehavior hilt = (WeaponHiltBehavior)hitObject;
                    if (hilt != null) hit = SkillTargetEnum.Hilt;
                    break;
                case SkillTargetEnum.Walls:
                    int layer = LayerMask.NameToLayer("Walls");
                    Collider2D obj = (Collider2D)hitObject;
                    if (obj != null && obj.gameObject.layer == layer) hit = SkillTargetEnum.Walls;
                    break;
                case SkillTargetEnum.Blade:
                    WeaponBladeBehavior blade = (WeaponBladeBehavior)hitObject;
                    if (blade != null) hit = SkillTargetEnum.Blade;
                    break;
                default:
                    break;
            }
        }

        return hit;
    }

    /// <summary>
    /// Checks if currentUseCount is greater than Max Use Count.
    /// </summary>
    /// <returns>Returns False if Max Use has been reached.</returns>
    public virtual bool isMaxUsageReached()
    {
        if (hasMaxUsage)
        {
            if (currentUseCount >= maxUseCount)
            {
                return true;
            }
        }
        return false;
    }

    internal virtual void IncrementMaxUsage()
    {
        if (hasMaxUsage)
        {
            if (!isMaxUsageReached())
            {
                currentUseCount++;
            }
        }

        if (isMaxUsageReached())
        {
            onMaxUsageReached.ForEach(x => x.Invoke());
        }
    }

    public virtual bool isSkillOnCooldown()
    {
        if (hasCooldown)
        {
            return (currentCooldown < cooldown);
        }

        return false;
    }

    internal virtual void SetSkillOnCooldown(bool value)
    {
        cooldownStarted = value;

        if (cooldownStarted)
        {
            // Checks if Cooldown is ongoing, if not, call for the updates currently occurring.
            if(!isSkillOnCooldown() && onCooldownUpdate.Count > 0) 
            { 
                onCooldownUpdate.ForEach(x => x.Invoke());
            }
        }
    }

    public virtual void AddMaxUsageReachedCallback(Action action)
    {
        onMaxUsageReached.Add(action);
    }
    
    public virtual void AddOnCooldownUpdateCallback(Action action)
    {
        onCooldownUpdate.Add(action);
    }
    
    public virtual void AddOnCooldownFinishCallback(Action action)
    {
        onCooldownFinish.Add(action);
    }

    public virtual void RemoveCallbacks()
    {
        onCooldownFinish.Clear();
        onCooldownUpdate.Clear();
        onMaxUsageReached.Clear();
    }
}
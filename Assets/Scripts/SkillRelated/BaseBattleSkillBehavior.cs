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
    public List<SkillDetectionPartEnum> skillDetectionPartEnums = new List<SkillDetectionPartEnum>();

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
                cooldownStarted = false;
                currentCooldown = 0.0f;

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

        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_DETECTION_TO_ALL_PARTS))
        {
            skillDetectionPartEnums.Add(SkillDetectionPartEnum.AllParts);
        }
        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_DETECTION_TO_BLADE_ONLY))
        {
            skillDetectionPartEnums.Add(SkillDetectionPartEnum.BladeOnlyDetection);
        }
        if (skillData.skillValues.ContainsKey(SkillVariableNames.SET_DETECTION_TO_HILT_ONLY))
        {
            skillDetectionPartEnums.Add(SkillDetectionPartEnum.HiltOnlyDetection);
        }

    }

    /// <summary>
    /// Method Checks if the object is in the list of target objects that can trigger this skill.
    /// </summary>
    /// <param name="objectType">Provides the type object that hit</param>
    /// <returns>Returns True if its in the list.</returns>
    public virtual bool IsObjectInListOfTargetValues(SkillTargetEnum objectType)
    {
        return (skillTargetEnums.Contains(objectType));
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
        if (hasCooldown && cooldownStarted)
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
            // Checks if Cooldown is ongoing, call for the updates currently occurring.
            if(isSkillOnCooldown() && onCooldownUpdate.Count > 0) 
            { 
                onCooldownUpdate.ForEach(x => x.Invoke());
            }
        }
        else
        {
            currentCooldown = 0.0f;
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
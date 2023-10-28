using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using User.Data;

public class TrainingSkillSlotsBehavior : AnimationMonoBehavior
{
    public List<SpriteRenderer> fillerSlotIconList;
    public SpriteRenderer originalCurrentSkillSlot;
    public SpriteRenderer fakeCurrentSkillSlot;

    bool isSlotSpinning = false;

    public Action onSlotClicked;
    public void SlotClicked()
    {
        if(!isSlotSpinning && UserDataBehavior.DoesUserHaveSkillPill())
        {
            isSlotSpinning =true;
            Play("SpinSlots", () => { isSlotSpinning = false; });

            if(onSlotClicked != null)
            {
                onSlotClicked.Invoke();
            }
        }
        else
        {
            Play("SpinFail");
        }
    }

    public void OnMouseDown()
    {
        SlotClicked();
    }

    public void SetupSkillSlot(SkillData skillData)
    {
        if(skillData == null)
        {
            originalCurrentSkillSlot.sprite = 
                DataVaultManager.Instance.GetSkillSprite("Empty_Icon");
        }
        else
        {
            originalCurrentSkillSlot.sprite = 
                DataVaultManager.Instance.GetSkillSprite(skillData.skillName + "_Icon");
        }
    }
}

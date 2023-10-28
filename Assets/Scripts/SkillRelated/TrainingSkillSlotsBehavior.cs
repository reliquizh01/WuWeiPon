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
    bool stillProcessing = false;

    private SkillData currentSkillData;

    private UserTransactionResultEnums currentTransactionResult;

    public void SlotClicked()
    {
        if(!isSlotSpinning && UserDataBehavior.DoesUserHaveSkillPill())
        {
            isSlotSpinning =true;
            currentTransactionResult = UserDataBehavior.PurchaseSkill(currentSkillData);

            SetupTransaction();
        }
        else
        {
            if (!stillProcessing)
            {
                Play("SpinFail");
            }
        }
    }

    public void OnMouseDown()
    {
        SlotClicked();
    }

    internal void SetupTransaction()
    {
        switch (currentTransactionResult)
        {
            case UserTransactionResultEnums.SkillPurchaseSameSkill:
                // TODO ADD LEVELUP ACTION
                break;
            case UserTransactionResultEnums.SkillPurchaseFailed:
                // TODO: CHECK NETWORK CONNECTION ONCE WE SETUP DATABASE
                break;
            case UserTransactionResultEnums.SkillPurchaseAwaitingConfirmation:
                PlaySpinSlots(UserDataBehavior.ObtainWaitingConfirmatonPurchase());
                break;
            case UserTransactionResultEnums.SkillPurchaseSuccesToEquip:
                PlaySpinSlots(UserDataBehavior.ConfirmSkillPurchase());
                break;
        }
    }

    internal void PlaySpinSlots(SkillData generatedSkill)
    {
        List<Sprite> sprites = DataVaultManager.Instance.GetUniqueSkillSprites(fillerSlotIconList.Count);

        for (int i = 0; i < fillerSlotIconList.Count; i++)
        {
            fillerSlotIconList[i].sprite = sprites[i];
        }

        if(currentSkillData != null)
        {
            fakeCurrentSkillSlot.sprite = DataVaultManager.Instance.GetSkillSprite(currentSkillData.skillIconFileName);
        }
        else
        {
            fakeCurrentSkillSlot.sprite = sprites[sprites.Count - 1];
        }

        originalCurrentSkillSlot.sprite = DataVaultManager.Instance.GetSkillSprite(generatedSkill.skillIconFileName);

        Play("SpinSlots", EndSpinSlots);
    }

    internal void EndSpinSlots()
    {
        isSlotSpinning = false;
        stillProcessing = false;
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
                DataVaultManager.Instance.GetSkillSprite(skillData.skillIconFileName);

            currentSkillData = skillData;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using User.Data;
using System.Linq;

public class TrainingSkillSlotsBehavior : AnimationMonoBehavior
{
    public List<SpriteRenderer> fillerSlotIconList;
    public SpriteRenderer originalCurrentSkillSlot;
    public SpriteRenderer fakeCurrentSkillSlot;

    bool isSlotSpinning = false;
    bool stillProcessing = false;

    private SkillData currentSkillData;
    [SerializeField]internal int slotNumber;

    internal UserTransactionResultEnums currentTransactionResult;
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

    public void SlotClicked()
    {
        if(!isSlotSpinning && UserDataBehavior.DoesUserHaveSkillPill())
        {
            isSlotSpinning =true;
            currentTransactionResult = UserDataBehavior.PurchaseSkill(slotNumber);

            finalizedPurchase();
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

    private void finalizedPurchase()
    {
        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();
        int lastSkillAdded = weapon.skills.Count - 1;

        switch (currentTransactionResult)
        {
            case UserTransactionResultEnums.PurchasedSkillEquipped:
                PlaySpinSlots(weapon.skills[lastSkillAdded]);
                break;
            case UserTransactionResultEnums.PurchasedSkillExists:
                PlaySpinSlots(weapon.skills[lastSkillAdded]);
                break;
            case UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation:
                PlaySpinSlots(weapon.skillPurchased);
                break;
            case UserTransactionResultEnums.PurchaseFailed:
                break;
            default:
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

        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();

        switch (currentTransactionResult)
        {
            case UserTransactionResultEnums.PurchasedSkillEquipped:
                break;
            case UserTransactionResultEnums.PurchasedSkillExists:
                break;
            case UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation:
                SkillData skillInThisSlot = weapon.skills.First(x => x.slotNumber == slotNumber);
                SkillPurchasePopUpContainer.Instance.SetupSkillPurchase(skillInThisSlot, weapon.skillPurchased, this);
                break;
            case UserTransactionResultEnums.PurchaseFailed:
                break;
            default:
                break;
        }
    }

    internal void ContinueLastTransaction()
    {
        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();
        currentTransactionResult = UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation;
        PlaySpinSlots(weapon.skillPurchased);
    }
}

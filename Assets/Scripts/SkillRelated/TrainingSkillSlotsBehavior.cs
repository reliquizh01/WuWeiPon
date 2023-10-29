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

    public BaseNotificationBehavior expNotif;

    bool isOtherSlotSpinning
    {
        get
        {
            return (GameManager.Instance.equippedWeaponContainer.weaponSlotsContainer.skillSlots.FirstOrDefault(x => x.isSlotSpinning) != null);
        }
    }
    bool isSlotSpinning = false;
    bool stillProcessing = false;

    private SkillData currentSkillData;
    [SerializeField]internal int slotNumber;

    internal UserTransactionResultEnums currentTransactionResult;

    float clickCounter = 0.0f;
    bool isClicked = false;
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
        if(!isOtherSlotSpinning && !isSlotSpinning && UserDataBehavior.DoesUserHaveSkillPill())
        {
            isSlotSpinning =true;
            currentTransactionResult = UserDataBehavior.PurchaseSkill(slotNumber);

            finalizedPurchase();
        }
        else
        {
            if (!stillProcessing && !isSlotSpinning)
            {
                Play("SpinFail");
            }
        }
    }

    private void Update()
    {
        if (isClicked)
        {
            clickCounter += Time.deltaTime;
            if(clickCounter > 0.5f)
            {
                // Show Skill Information
                isClicked = false;
            }
        }
    }
    public void OnMouseDown()
    {
        isClicked = true;
    }

    public void OnMouseUp()
    {
        if(clickCounter <= 0.5f)
        {
            SlotClicked();
        }
        else
        {
            //Hide Skill Information
        }

        clickCounter = 0.0f;
        isClicked = false;
    }

    public void PlayUpgrade()
    {
        expNotif.Play();
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
                PlaySpinSlots(weapon.lastUpgradedSkill);
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
                SkillData equipThisSkill = weapon.skills[weapon.skills.Count - 1];
                SetupSkillSlot(equipThisSkill);
                break;
            case UserTransactionResultEnums.PurchasedSkillExists:
                SkillData skillToUpgrade = weapon.skills.First(x => x.skillName == weapon.lastUpgradedSkill.skillName);
                GameManager.Instance.equippedWeaponContainer.weaponSlotsContainer.UpgradeSkillSlot(skillToUpgrade);
                SetupSkillSlot(currentSkillData);
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

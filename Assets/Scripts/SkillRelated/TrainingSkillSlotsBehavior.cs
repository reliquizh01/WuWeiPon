using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using User.Data;
using System.Linq;
using UnityEngine.EventSystems;

public class TrainingSkillSlotsBehavior : AnimationMonoBehavior
{
    public List<SpriteRenderer> fillerSlotIconList;
    public SpriteRenderer originalCurrentSkillSlot;
    public SpriteRenderer fakeCurrentSkillSlot;

    public BaseSpriteRendererNotificationBehavior expNotif;
    public AudioContainer audioContainer;
    public AudioContainer audioSfx;

    bool isOtherSlotSpinning
    {
        get
        {
            return (GameManager.Instance.equippedWeaponContainer.weaponSlotsContainer.skillSlots.FirstOrDefault(x => x.isSlotSpinning) != null);
        }
    }

    bool isSlotSpinning = false;
    bool stillProcessing = false;
    bool lastAutomationSetup = false;
    List<SkillRankEnum> lastAutomationSkillRankSetup = new List<SkillRankEnum>();

    private SkillData currentSkillData;
    [SerializeField]internal int slotNumber;

    internal UserTransactionResultEnums currentTransactionResult;

    float clickCounter = 0.0f;
    bool isClicked = false;
    public void SetupSkillSlotVisuals(SkillData skillData)
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

            lastAutomationSkillRankSetup = UserDataBehavior.GetAutomationRankSkills();
            lastAutomationSetup = UserDataBehavior.IsSkillSpinAutomated();
        }
    }

    public void SlotClicked()
    {
        if(!isOtherSlotSpinning && 
           !isSlotSpinning && UserDataBehavior.DoesUserHaveSkillPill())
        {
            isSlotSpinning =true;
            currentTransactionResult = UserDataBehavior.PurchaseSkill(slotNumber);

            finalizedPurchase();
            audioContainer.SetAndPlay("SpinSkillSlot");
        }
        else
        {
            if (currentSkillData != null && !stillProcessing && !isSlotSpinning)
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
            if(clickCounter > 0.35f)
            {
                // Show Skill Information
                InformationPopUpContainer.Instance.SetupSkillInformation(currentSkillData);
                isClicked = false;
            }
        }
    }

    public void OnMouseDown()
    {
        if (!SkillPurchasePopUpContainer.Instance.popUpContainer.activeInHierarchy &&
            !AutomationSettingsBehavior.Instance.container.activeInHierarchy)
        {
            isClicked = true;
        }
    }

    public void OnMouseUp()
    {
        if(clickCounter <= 0.35f && isClicked)
        {
            SlotClicked();
        }
        else
        {
            //Hide Skill Information
            InformationPopUpContainer.Instance.HideInformation();
        }

        clickCounter = 0.0f;
        isClicked = false;
    }

    public void PlayUpgrade()
    {
        expNotif.Play();
        audioSfx.SetAndPlay("SpinSkillResult");
    }

    private void finalizedPurchase()
    {
        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();
        int lastSkillAdded = weapon.skills.Count - 1;

        switch (currentTransactionResult)
        {
            case UserTransactionResultEnums.PurchasedSkillGetsEquipped:
                PlaySpinSlots(weapon.skills[lastSkillAdded]);
                break;
            case UserTransactionResultEnums.PurchaseSkillHasExistingCopyInWeaponSkills:
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

        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();

        AnalyzeTransactionResult(weapon);
    }

    internal void AnalyzeTransactionResult(WeaponData weapon) 
    {
        switch (currentTransactionResult)
        {
            case UserTransactionResultEnums.PurchasedSkillGetsEquipped:
                SkillData equipThisSkill = weapon.skills[weapon.skills.Count - 1];
                SetupSkillSlotVisuals(equipThisSkill);
                CheckSkillSlotAutomation();
                break;
            case UserTransactionResultEnums.PurchaseSkillHasExistingCopyInWeaponSkills:
                SkillData skillToUpgrade = weapon.skills.First(x => x.skillName == weapon.lastUpgradedSkill.skillName);
                GameManager.Instance.equippedWeaponContainer.weaponSlotsContainer.UpgradeSkillSlot(skillToUpgrade);
                SetupSkillSlotVisuals(currentSkillData);
                CheckSkillSlotAutomation();
                break;
            case UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation:
                if (UserDataBehavior.IsSkillSpinAutomated() &&
                    lastAutomationSkillRankSetup.Contains(weapon.skillPurchased.skillRank))
                {
                    CheckSkillSlotAutomation();
                }
                else
                {
                    SkillData skillInThisSlot = weapon.skills.First(x => x.slotNumber == slotNumber);
                    SkillPurchasePopUpContainer.Instance.SetupSkillPurchase(skillInThisSlot, weapon.skillPurchased, this);
                }

                break;
            case UserTransactionResultEnums.PurchaseFailed:
                break;
            default:
                break;
        }
    }

    public void CheckSkillSlotAutomation()
    {
        stillProcessing = false;

        if (lastAutomationSetup && GameManager.Instance.currentGameState == GameStateEnum.Idle)
        {
            if (UserDataBehavior.DoesUserHaveSkillPill())
            {
                SlotClicked();
            }
            else
            {
                AutomationSettingsBehavior.Instance.skillAutomationToggle.ToggleAutomation();
            }
        }

        lastAutomationSkillRankSetup = UserDataBehavior.GetAutomationRankSkills();
        lastAutomationSetup = UserDataBehavior.IsSkillSpinAutomated();
    }

    internal void ContinueLastTransaction()
    {
        WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();
        currentTransactionResult = UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation;
        PlaySpinSlots(weapon.skillPurchased);
    }
}

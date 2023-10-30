using DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using SkillRelated;

namespace User.Data
{
    public static class UserDataBehavior
    {
        internal static UserData currentUserData;

        internal const int spiritualEssenceForSkillPull = 15;

        public static void LoadUser(UserData userData)
        {
            currentUserData = new UserData(userData);
        }

        #region OnBoarding 

        public static void AddOnboardingProgress(PlayerOnBoardingEnum newOnboarding)
        {
            currentUserData.playerOnBoardingProgress.Add(newOnboarding);

            SaveLoadManager.SaveUser();
        }

        #endregion OnBoarding

        public static void AddNewWeapon(WeaponData weaponData)
        {
            if (GetPlayerEquippedWeapon() != null)
            {
                weaponData.isEquipped = true;
            }

            currentUserData.weapons.Add(weaponData);

            SaveLoadManager.SaveUser();
        }

        public static WeaponData GetPlayerEquippedWeapon()
        {
            return currentUserData.weapons.Find(x => x.isEquipped);
        }

        public static int GetCurrency(CurrencyEnum currency)
        {
            if(currentUserData == null)
            {
                return -1;
            }

            switch (currency)
            {
                case CurrencyEnum.spirtualEssence:
                    return currentUserData.spiritualEssence;
                case CurrencyEnum.skillPills:
                    return currentUserData.skillPills;
                case CurrencyEnum.fateGems:
                    return currentUserData.fateGems;
                case CurrencyEnum.consistencyPills:
                    return currentUserData.consistencyPills;
                default:
                    break;
            }

            return -1;
        }

        #region Skill Purchase

        public static bool DoesUserHaveSkillPill()
        {
            return currentUserData.skillPills > 0;
        }

        public static UserTransactionResultEnums PurchaseSkill(int slotNumber)
        {
            UserTransactionResultEnums transactionResult = UserTransactionResultEnums.PurchaseFailed;

            WeaponData weapon = GetPlayerEquippedWeapon();

            SkillData skillEquippedInSlotNumberProvided = weapon.skills.FirstOrDefault(x => x.slotNumber == slotNumber);

            SkillData generatePurchasedSkill = SkillGenerator.GenerateSkill();

            if(weapon.skills.Find(x => x.skillName == generatePurchasedSkill.skillName) != null)
            {
                int upgradeSkillSlotNumber = weapon.skills.FindIndex(x => x.skillName == generatePurchasedSkill.skillName);
                UpgradeExistingSkill(generatePurchasedSkill, upgradeSkillSlotNumber);

                weapon.lastUpgradedSkill = new SkillData(generatePurchasedSkill);
                weapon.lastUpgradedSkill.slotNumber = upgradeSkillSlotNumber;

                addSkillPillNoSave(-1);

                addSpiritualEssenceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillExists;
            }
            else if (skillEquippedInSlotNumberProvided == null)
            {
                weapon.skills.Add(new SkillData(generatePurchasedSkill));
                weapon.skills[weapon.skills.Count - 1].slotNumber = slotNumber;

                addSkillPillNoSave(-1);

                addSpiritualEssenceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillEquipped;
            }
            else if(skillEquippedInSlotNumberProvided != null)
            {
                weapon.skillPurchased = new SkillData(generatePurchasedSkill);
                weapon.skillPurchased.slotNumber = slotNumber;

                addSkillPillNoSave(-1);

                addSpiritualEssenceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillOnFilledSkillSlotNeedsConfirmation;
            }
            else
            {
                transactionResult = UserTransactionResultEnums.PurchaseFailed;
            }

            SaveLoadManager.SaveUser();
            return transactionResult;
        }

        public static void PlayerChooseCurrentSkill(int slotNumber)
        {
            WeaponData weapon = GetPlayerEquippedWeapon();
            weapon.skillPurchased = null;

            SaveLoadManager.SaveUser();
        }

        public static void PlayerChooseNewSkill(int slotNumber)
        {
            WeaponData weapon = GetPlayerEquippedWeapon();
            SkillData currentSkill = weapon.skills.First(x => x.slotNumber == slotNumber);
            weapon.skills.Remove(currentSkill);
            weapon.skills.Add(new SkillData(weapon.skillPurchased));
            weapon.skillPurchased = null;

            SaveLoadManager.SaveUser();
        }

        public static void UpgradeExistingSkill(SkillData sacrificedSkill, int slotNumber)
        {
            WeaponData weapon = GetPlayerEquippedWeapon();
            weapon.skills[slotNumber].skillLevel += 1;

            SaveLoadManager.SaveUser();
        }

        #endregion Skill Purchase

        #region Currency

        private static void addSpiritualEssenceNoSave(int amount)
        {
            currentUserData.spiritualEssence += amount;
            GameManager.Instance.UpdateCurrencyValues(CurrencyEnum.spirtualEssence);
        }

        private static void addSkillPillNoSave(int amount)
        {
            currentUserData.skillPills += amount;
            GameManager.Instance.UpdateCurrencyValues(CurrencyEnum.skillPills);
        }


        #endregion Currency
    }
}
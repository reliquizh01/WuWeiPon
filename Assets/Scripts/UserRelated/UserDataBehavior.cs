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
        public static void AddOnboardingProgress(PlayerOnBoardingEnum newOnboarding)
        {
            currentUserData.playerOnBoardingProgress.Add(newOnboarding);

            SaveLoadManager.SaveUser();
        }

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

                currentUserData.skillPills--;
                AddSpiritualExperienceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillExists;
            }
            else if (skillEquippedInSlotNumberProvided == null)
            {
                weapon.skills.Add(new SkillData(generatePurchasedSkill));
                weapon.skills[weapon.skills.Count - 1].slotNumber = slotNumber;

                currentUserData.skillPills--;
                AddSpiritualExperienceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillEquipped;
            }
            else if(skillEquippedInSlotNumberProvided != null)
            {
                weapon.skillPurchased = new SkillData(generatePurchasedSkill);
                weapon.skillPurchased.slotNumber = slotNumber;

                currentUserData.skillPills--;
                AddSpiritualExperienceNoSave(spiritualEssenceForSkillPull);

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

        #region Spiritual Essence

        private static void AddSpiritualExperienceNoSave(int amount)
        {
            currentUserData.spiritualEssence += amount;
        }

        #endregion
    }
}
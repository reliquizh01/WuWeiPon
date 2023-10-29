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
                AddLevelToExistingSkill(generatePurchasedSkill);
                currentUserData.skillPills--;

                transactionResult = UserTransactionResultEnums.PurchasedSkillExists;
            }
            else if (skillEquippedInSlotNumberProvided == null)
            {
                weapon.skills.Add(new SkillData(generatePurchasedSkill));
                weapon.skills[weapon.skills.Count - 1].slotNumber = slotNumber;
                currentUserData.skillPills--;

                transactionResult = UserTransactionResultEnums.PurchasedSkillEquipped;
            }
            else if(skillEquippedInSlotNumberProvided != null)
            {
                weapon.skillPurchased = new SkillData(generatePurchasedSkill);
                weapon.skillPurchased.slotNumber = slotNumber;

                currentUserData.skillPills--;

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
        private static void AddLevelToExistingSkill(SkillData sacrificedSkill)
        {
            WeaponData weapon = GetPlayerEquippedWeapon();

            int skillIdx = weapon.skills.FindIndex(x => x.skillName == sacrificedSkill.skillName);

            weapon.skills[skillIdx].skillLevel += 1;
        }

        #endregion Skill Purchase
    }
}
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
    
        public static bool DoesUserHaveSkillPill()
        {
            return currentUserData.skillPills > 0;
        }

        public static UserTransactionResultEnums PurchaseSkill(SkillData skillSlotInQuestion)
        {
            UserTransactionResultEnums result = UserTransactionResultEnums.SkillPurchaseAwaitingConfirmation;

            WeaponData refCurrentEquippedWeapon = currentUserData.weapons.First(x => x.isEquipped);
                

            if(skillSlotInQuestion != null)
            {
                refCurrentEquippedWeapon.skillSlotInQuestion = skillSlotInQuestion;
                refCurrentEquippedWeapon.skillPurchased = SkillGenerator.GenerateSkill();

                if(refCurrentEquippedWeapon.skillPurchased.skillName == refCurrentEquippedWeapon.skillSlotInQuestion.skillName)
                {
                    refCurrentEquippedWeapon.skillSlotInQuestion.skillLevel++;
                    result = UserTransactionResultEnums.SkillPurchaseSameSkill;
                }
            }
            else
            {
                refCurrentEquippedWeapon.skillPurchased = SkillGenerator.GenerateSkill();
                result = UserTransactionResultEnums.SkillPurchaseSuccesToEquip;
            }

            currentUserData.skillPills -= 1;

            SaveLoadManager.SaveUser();

            return result;
        }

        public static void EquipPurchasedSkill()
        {

        }

        public static SkillData ObtainWaitingConfirmatonPurchase()
        {
            WeaponData refCurrentEquippedWeapon = currentUserData.weapons.First(x => x.isEquipped);
            
            SkillData skillData = new SkillData(refCurrentEquippedWeapon.skillPurchased);

            return skillData;
        }
        public static SkillData ConfirmSkillPurchase()
        {
            WeaponData refCurrentEquippedWeapon = currentUserData.weapons.First(x => x.isEquipped);
            refCurrentEquippedWeapon.skills.Add(refCurrentEquippedWeapon.skillPurchased);

            SkillData skillData = new SkillData(refCurrentEquippedWeapon.skillPurchased);

            refCurrentEquippedWeapon.skillPurchased = null;

            SaveLoadManager.SaveUser();

            return skillData;
        }
    }
}
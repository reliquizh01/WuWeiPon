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

        #region Weapon Training

        internal static void AddSpiritCondensation(SpiritCondensation newCondensation)
        {
            currentUserData.currentSpiritCondensation = newCondensation;
            SaveLoadManager.SaveUser();
        }

        internal static SpiritCondensation GetUserCurrentCondensation()
        {
            return currentUserData.currentSpiritCondensation;
        }

        internal static void IncreaseCurrentEquippedWeaponStat(WeaponStatEnum weaponStat, float amount, bool save = false)
        {
            WeaponData currentWeapon = GetPlayerEquippedWeapon();

            switch (weaponStat)
            {
                case WeaponStatEnum.weapon_Health:
                    currentWeapon.weapon_Health += amount;
                    break;
                case WeaponStatEnum.damage_Physical:
                    currentWeapon.damage_Physical += amount;
                    break;
                case WeaponStatEnum.damage_Magic:
                    currentWeapon.damage_Magic += amount;
                    break;
                case WeaponStatEnum.cooldown_Reduction:
                    currentWeapon.cooldown_Reduction += amount;
                    break;
                case WeaponStatEnum.armor_Penetration:
                    currentWeapon.armor_Penetration += amount;
                    break;
                case WeaponStatEnum.armor_Physical:
                    currentWeapon.armor_Physical += amount;
                    break;
                case WeaponStatEnum.armor_Magic:
                    currentWeapon.armor_Magic += amount;
                    break;
                case WeaponStatEnum.status_Resistance:
                    currentWeapon.status_Resistance += amount;
                    break;
                case WeaponStatEnum.poison_Resistance:
                    currentWeapon.poison_Resistance += amount;
                    break;
                case WeaponStatEnum.monster_Damage:
                    currentWeapon.monster_Damage += amount;
                    break;
                case WeaponStatEnum.luck:
                    currentWeapon.luck += amount;
                    break;
                case WeaponStatEnum.evasion:
                    currentWeapon.evasion += amount;
                    break;
                case WeaponStatEnum.spin_Speed:
                    currentWeapon.spin_Speed += amount;
                    break;
                case WeaponStatEnum.critChance:
                    currentWeapon.critChance += amount;
                    break;
                case WeaponStatEnum.critPercentDamage:
                    currentWeapon.critPercentDamage += amount;
                    break;
                default:
                    break;
            }

            if(save)
            {
                SaveLoadManager.SaveUser();
            }
        }

        internal static void RemoveCurrentSpiritCondensation()
        {
            currentUserData.currentSpiritCondensation = null;
            SaveLoadManager.SaveUser();
        }

        #endregion Weapon Training

        #region Currency

        internal static void addSpiritualEssence(int amount)
        {
            currentUserData.spiritualEssence += amount;
            GameManager.Instance.UpdateCurrencyValues(CurrencyEnum.spirtualEssence);            
        }

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
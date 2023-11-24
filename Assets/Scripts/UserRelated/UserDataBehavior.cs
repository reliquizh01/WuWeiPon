using DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using SkillRelated;
using UnityEngine.Profiling;

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
            if (GetPlayerEquippedWeapon() == null)
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

        public static int GetPlayerOnBoardingCount()
        {
            return currentUserData.playerOnBoardingProgress.Count;
        }

        public static float GetCurrentEquippedWeaponStat(WeaponStatEnum weaponStatEnum)
        {
            WeaponData weaponData = GetPlayerEquippedWeapon();
            float statAmount = 0;

            switch (weaponStatEnum)
            {
                case WeaponStatEnum.weapon_Health:
                    statAmount = weaponData.weapon_Health;
                    break;
                case WeaponStatEnum.damage_Physical:
                    statAmount = weaponData.damage_Physical;
                    break;
                case WeaponStatEnum.damage_Magic:
                    statAmount = weaponData.damage_Magic;
                    break;
                case WeaponStatEnum.cooldown_Reduction:
                    statAmount = weaponData.cooldown_Reduction;
                    break;
                case WeaponStatEnum.armor_Penetration:
                    statAmount = weaponData.armor_Penetration;
                    break;
                case WeaponStatEnum.armor_Physical:
                    statAmount = weaponData.armor_Physical;
                    break;
                case WeaponStatEnum.armor_Magic:
                    statAmount = weaponData.armor_Magic;
                    break;
                case WeaponStatEnum.status_Resistance:
                    statAmount = weaponData.status_Resistance;
                    break;
                case WeaponStatEnum.poison_Resistance:
                    statAmount = weaponData.poison_Resistance;
                    break;
                case WeaponStatEnum.monster_Damage:
                    statAmount = weaponData.monster_Damage;
                    break;
                case WeaponStatEnum.luck:
                    statAmount = weaponData.luck;
                    break;
                case WeaponStatEnum.evasion:
                    statAmount = weaponData.evasion;
                    break;
                case WeaponStatEnum.spin_Speed:
                    statAmount = weaponData.spin_Speed;
                    break;
                case WeaponStatEnum.critChance:
                    statAmount = weaponData.critChance;
                    break;
                case WeaponStatEnum.critPercentDamage:
                    statAmount = weaponData.critPercentDamage;
                    break;
                default:;
                    break;
            }

            return statAmount;
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

                transactionResult = UserTransactionResultEnums.PurchaseSkillHasExistingCopyInWeaponSkills;
            }
            else if (skillEquippedInSlotNumberProvided == null)
            {
                weapon.skills.Add(new SkillData(generatePurchasedSkill));
                weapon.skills[weapon.skills.Count - 1].slotNumber = slotNumber;

                addSkillPillNoSave(-1);

                addSpiritualEssenceNoSave(spiritualEssenceForSkillPull);

                transactionResult = UserTransactionResultEnums.PurchasedSkillGetsEquipped;
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

        public static SkillData GetCurrentEquippedPurchaseSkill()
        {
            WeaponData weaponData = currentUserData.weapons.FirstOrDefault(x => x.isEquipped);
            return (weaponData != null && weaponData.skillPurchased != null) ? weaponData.skillPurchased : null;
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

        #region Automation Skills

        internal static List<SkillRankEnum> GetAutomatedSkillRarity()
        {
            return currentUserData.userSettingsData.acceptableSkillRankList;
        }

        internal static bool IsSkillSpinAutomated()
        {
            return currentUserData.userSettingsData.isAutoRollSkllsEnabled;
        }

        internal static bool IsSkillRankFilteredToIgnorInAutomation(SkillRankEnum skillRank)
        {
            return currentUserData.userSettingsData.acceptableSkillRankList.Contains(skillRank);
        }

        internal static List<SkillRankEnum> GetAutomationRankSkills()
        {
            return new List<SkillRankEnum>(currentUserData.userSettingsData.acceptableSkillRankList);
        }

        internal static void ToggleAutomation(bool setTo)
        {
            currentUserData.userSettingsData.isAutoRollSkllsEnabled = setTo;
        }

        #endregion Automation Skills

        #region Battle Record Logs

        internal static void SaveBattleRecordLogs(BattleRecordLogs battleRecordLogs)
        {
            currentUserData.userBattleRecordsData.battleRecordLogs.Add(battleRecordLogs);

            SaveLoadManager.SaveUser();
        }

        internal static float GetHighestRecordedStats(RecordStatsEnum stats)
        {
            if(getHighestTransaction(stats) != null)
            {
                return getHighestTransaction(stats).amount;
            }
            else
            {
                return 0;
            }

        }

        private static BattleTransaction getHighestTransaction(RecordStatsEnum stats)
        {
            List<BattleRecordLogs> records = currentUserData.userBattleRecordsData.battleRecordLogs;

            BattleTransaction currentHighest = null;

            if (records.Count > 0)
            {
                List<BattleTransaction> transactions = new List<BattleTransaction>();
    
                records.ForEach(record =>
                {
                    transactions.AddRange(record.battleTransactionLog.Where(log => log.recordStats == stats).ToList());
                });

                transactions.ForEach(record =>
                {
                    if (currentHighest == null) currentHighest = record;
                    else if (record.amount > currentHighest.amount)
                    {
                        currentHighest = record;
                    }
                });

            }

            return currentHighest;
        }

        internal static void SetHighestDamageTakenRecordNoSave(BattleRecordLogs battleLog)
        {
            BattleRecordLogs highestDamageTaken = currentUserData.userBattleRecordsData.highestDamageTakenLog;
            if (highestDamageTaken != null)
            {
                float highestTotalDamage = highestDamageTaken.battleTransactionLog.First(
                    x => x.recordStats == RecordStatsEnum.DamageTaken).amount;

                if (highestTotalDamage < battleLog.battleTransactionLog.First(x => x.recordStats == RecordStatsEnum.DamageTaken).amount)
                {
                    currentUserData.userBattleRecordsData.highestDamageTakenLog = new BattleRecordLogs(battleLog);
                }

            }
            else
            {
                currentUserData.userBattleRecordsData.highestDamageTakenLog = new BattleRecordLogs(battleLog);
            }
        }

        internal static void SetOneHitDamageDealtRecordNoSave(BattleRecordLogs battleLog)
        {
            BattleRecordLogs oneHitRecord = currentUserData.userBattleRecordsData.highestOneHitDamage;
            if (oneHitRecord != null)
            {
                float highestTotalDamage = oneHitRecord.battleTransactionLog.First(
                    x => x.recordStats == RecordStatsEnum.HighestOneHitDamage).amount;

                if (highestTotalDamage < battleLog.battleTransactionLog.First(x => x.recordStats == RecordStatsEnum.HighestOneHitDamage).amount)
                {
                    currentUserData.userBattleRecordsData.highestOneHitDamage = new BattleRecordLogs(battleLog);
                }

            }
            else
            {
                currentUserData.userBattleRecordsData.highestOneHitDamage = new BattleRecordLogs(battleLog);
            }
        }

        internal static void SetTotalDamageDealtRecordNoSave(BattleRecordLogs battleLog)
        {
            BattleRecordLogs highestTotalDamage = currentUserData.userBattleRecordsData.highestTotalDamage;
            if (highestTotalDamage != null)
            {
                float totalDmg = highestTotalDamage.battleTransactionLog.First(
                    x => x.recordStats == RecordStatsEnum.TotalDamage).amount;

                if(totalDmg < battleLog.battleTransactionLog.First(x => x.recordStats == RecordStatsEnum.TotalDamage).amount)
                {
                    currentUserData.userBattleRecordsData.highestTotalDamage = new BattleRecordLogs(battleLog);
                }

            }
            else
            {
                currentUserData.userBattleRecordsData.highestTotalDamage = new BattleRecordLogs(battleLog);
            }
        }

        #endregion Battle Record Logs
    }
}
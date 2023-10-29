using System.Collections.Generic;
using System.Collections;
using Identity.Randomizer;
using System;
using UnityEngine;

namespace WeaponRelated
{
    public class WeaponGenerator
    {
        public WeaponData GenerateWeapon(WeaponRankEnum weaponRank, WeaponTypeEnum type = WeaponTypeEnum.Dagger)
        {
            WeaponData newWeapon = new WeaponData()
            {
                weaponId = RandomIdentification.RandomString(15),
                weaponRank = weaponRank,
                damage_physical = RandomPhysicalPoints(weaponRank),
                damage_magic = RandomMagicPoints(weaponRank),
                weaponHealth = RandomHealthPoints(weaponRank),
                weaponType = type,
                behaviorSkillSlotCount = randomBehaviorSlotCount(weaponRank, type),
                attributeSlotCount = randomAttributeSlotCount(weaponRank, type),
            };

            return newWeapon;
        }

        private int randomBehaviorSlotCount(WeaponRankEnum weaponRank, WeaponTypeEnum type)
        {
            int totalCount = 0;

            switch (weaponRank)
            {
                case WeaponRankEnum.ordinary:
                    totalCount = UnityEngine.Random.Range(2, 3);
                    break;
                case WeaponRankEnum.rare:
                    break;
                case WeaponRankEnum.transcendant:
                    break;
                case WeaponRankEnum.ancient:
                    break;
                case WeaponRankEnum.divine:
                    break;
                case WeaponRankEnum.ancientDivine:
                    break;
                default:
                    break;
            }

            return totalCount;
        }

        private int randomAttributeSlotCount(WeaponRankEnum weaponRank, WeaponTypeEnum type)
        {
            int totalCount = 0;

            switch (weaponRank)
            {
                case WeaponRankEnum.ordinary:
                    totalCount = UnityEngine.Random.Range(1, 3);
                    break;
                case WeaponRankEnum.rare:
                    break;
                case WeaponRankEnum.transcendant:
                    break;
                case WeaponRankEnum.ancient:
                    break;
                case WeaponRankEnum.divine:
                    break;
                case WeaponRankEnum.ancientDivine:
                    break;
                default:
                    break;
            }

            return totalCount;
        }

        #region  Random Points

        internal float RandomMagicPoints(WeaponRankEnum weaponRank)
        {
            float points = 0;

            switch(weaponRank)
            {
                case WeaponRankEnum.ordinary:
                points = 0;
                break;

                case WeaponRankEnum.rare:
                points = UnityEngine.Random.Range(0.0f, 3.0f);
                break;
            }

            return points;
        }

        internal float RandomPhysicalPoints(WeaponRankEnum weaponRank)
        {
            float points = 0;

            switch(weaponRank)
            {
                case WeaponRankEnum.ordinary:
                points = UnityEngine.Random.Range(1.0f, 5.0f);
                break;

                case WeaponRankEnum.rare:
                points = UnityEngine.Random.Range(6.0f, 10.0f);
                break;
            }

            return points;
        }
        
        internal float RandomHealthPoints(WeaponRankEnum weaponRank)
        {
            float points = 0;

            switch (weaponRank)
            {
                case WeaponRankEnum.ordinary:
                points = UnityEngine.Random.Range(15.0f, 20.0f);
                    break;
                case WeaponRankEnum.rare:
                    break;
                case WeaponRankEnum.transcendant:
                    break;
                case WeaponRankEnum.ancient:
                    break;
                case WeaponRankEnum.divine:
                    break;
                case WeaponRankEnum.ancientDivine:
                    break;
                default:
                    break;
            }

            return points;
        }
        #endregion Random Points
    }

}
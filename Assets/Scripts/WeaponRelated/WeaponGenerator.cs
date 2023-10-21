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
                weaponType = type
            };

            return newWeapon;
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
        
        #endregion Random Points
    }

}
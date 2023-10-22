using DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
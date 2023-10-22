using System;
using System.Collections.Generic;


namespace User.Data
{
    public class UserData
    {
        public UserData() { }
        public UserData(UserData userData) 
        { 
            uniqueId = userData.uniqueId;
            email = userData.email;
            weapons = new List<WeaponData>(userData.weapons);
            currentTokenAuthentication = new string(userData.currentTokenAuthentication);
            lastServerUpdate = userData.lastServerUpdate;
            skillPills = userData.skillPills;
            playerOnBoardingProgress = new List<PlayerOnBoardingEnum>(userData.playerOnBoardingProgress);
        }

        public string uniqueId = "";
        public string email = "";

        public List<WeaponData> weapons = new List<WeaponData>();

        public string currentTokenAuthentication = "";
        public DateTime lastServerUpdate;

        public int skillPills = 0;

        public List<PlayerOnBoardingEnum> playerOnBoardingProgress = new List<PlayerOnBoardingEnum>();
    }
}
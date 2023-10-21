using System;
using System.Collections.Generic;


namespace User.Data
{
    public class UserData
    {
        public string uniqueId = "";
        public string email = "";

        public List<WeaponData> weapons = new List<WeaponData>();

        public string currentTokenAuthentication = "";
        public DateTime lastServerUpdate;

        public int skillPills = 0;

        public List<PlayerOnBoardingEnum> playerOnBoardingProgress = new List<PlayerOnBoardingEnum>();
    }
}
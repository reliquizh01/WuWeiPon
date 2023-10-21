using System;
using System.Collections.Generic;


namespace User.Data
{
    public class UserData
    {
        public string uniqueId;
        public string email;

        public List<WeaponData> weapons;

        public string currentTokenAuthentication;
        public DateTime lastServerUpdate;

        public int skillPills;

        public List<PlayerOnBoardingEnum> playerOnBoardingProgress;
    }
}
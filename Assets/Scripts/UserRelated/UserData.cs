using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace User.Data
{
    [DataContract]
    public class UserData
    {
        #region Constructors

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

        #endregion Constructors

        [DataMember]
        public string uniqueId = "";
        
        [DataMember]
        public string email = "";

        [DataMember]
        public List<WeaponData> weapons = new List<WeaponData>();

        [DataMember]
        public string currentTokenAuthentication = "";
 
        [DataMember]
        public DateTime lastServerUpdate;

        [DataMember]
        public int skillPills = 0;

        [DataMember]
        public List<PlayerOnBoardingEnum> playerOnBoardingProgress = new List<PlayerOnBoardingEnum>();
    }
}
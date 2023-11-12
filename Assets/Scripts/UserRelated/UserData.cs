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
            playerOnBoardingProgress = new List<PlayerOnBoardingEnum>(userData.playerOnBoardingProgress);

            currentSpiritCondensation = userData.currentSpiritCondensation;

            skillPills = userData.skillPills;
            spiritualEssence = userData.spiritualEssence;
            fateGems = userData.fateGems;
            consistencyPills = userData.consistencyPills;

            if(userData.userSettingsData != null)
            {
                userSettingsData = new UserSettingsData(userData.userSettingsData);
            }
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
        public int skillPills = 50000;

        [DataMember]
        public int spiritualEssence = 50000;

        [DataMember]
        public int fateGems = 1000;

        /// <summary>
        /// Currency that can be used to enhance the % rate of any RNG-based chest/treasures.
        /// How to Gain:
        /// +1 Consistency pill = 1 hour of playtime.
        /// +10 Consistency pill = 24 hours of playtime.
        /// +100 consistency pill = 1 week of playtime.
        /// 
        /// How to Use:
        /// 1 Consistency Pill
        /// +0.1% increase on Divine pull rate on chests/treasure
        /// +0.5% increase on Ancient pull rate on chests/treasure
        /// +2.5% increase on Heroic pull rate on chests/treasure
        /// -3.6% decrease on Rare/Ordinary pull rate on chests/treasure
        /// </summary>
        [DataMember]
        public int consistencyPills = 1000;

        [DataMember]
        public SpiritCondensation currentSpiritCondensation;

        [DataMember]
        public List<PlayerOnBoardingEnum> playerOnBoardingProgress = new List<PlayerOnBoardingEnum>();

        [DataMember]
        public UserSettingsData userSettingsData = new UserSettingsData();
    }
}
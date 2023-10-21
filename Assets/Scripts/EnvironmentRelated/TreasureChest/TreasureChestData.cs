using System;
using System.Collections.Generic;

namespace PlayerPulls.Chest
{
    public class TreasureChestData
    {
        internal string transactionId = "";
        internal int skillPillsAmount = 0;
        internal List<WeaponData> containedWeapon = new List<WeaponData>();
    }
}
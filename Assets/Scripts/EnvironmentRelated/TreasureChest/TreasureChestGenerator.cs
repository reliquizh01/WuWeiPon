using DataManagement;
using Identity.Randomizer;
using System;
using System.Collections.Generic;
using WeaponRelated;

namespace PlayerPulls.Chest
{
    internal static class TreasureChestGenerator
    {
        internal static TreasureChestData GetRandomChest()
        {
            TreasureRank rank = new TreasureRank();

            //TODO
            //If connected to the server, try to have the server 
            //do the algorithm in obtaining the treasure.
            if (!ServerCallManager.IsConnectedToServer)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (random > 50)
                {
                    rank = TreasureRank.rare;
                }
                else
                {
                    rank = TreasureRank.ordinary;
                }
            }

            return generateChest(rank);
        }

        /// <summary>
        /// Generate what a treasure chest contains.
        /// This needs to be transferred in the server so the server can do this sequence.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        private static TreasureChestData generateChest(TreasureRank rank)
        {
            TreasureChestData treasureChestData = new TreasureChestData();

            treasureChestData.transactionId = RandomIdentification.RandomString(15);

            switch (rank)
            {
                case TreasureRank.ordinary:
                    treasureChestData.skillPillsAmount = UnityEngine.Random.Range(3, 4);
                    break;

                case TreasureRank.rare:
                    treasureChestData.skillPillsAmount = UnityEngine.Random.Range(2, 6);

                    WeaponGenerator tmp = new WeaponGenerator();
                    treasureChestData.containedWeapon.Add(tmp.GenerateWeapon((WeaponRankEnum)rank));
                    break;

                case TreasureRank.transcendant:
                    treasureChestData.skillPillsAmount = UnityEngine.Random.Range(6, 8);
                    break;
                case TreasureRank.ancient:
                    treasureChestData.skillPillsAmount = UnityEngine.Random.Range(8, 10);
                    break;
                case TreasureRank.divine:
                    treasureChestData.skillPillsAmount = UnityEngine.Random.Range(10, 12);
                    break;
                case TreasureRank.ancientDivine:
                    treasureChestData.skillPillsAmount = 15;
                    break;
                default:
                    break;
            }

            return treasureChestData;
        }
    }
}
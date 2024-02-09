using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using User.Data;
using System.IO;
using Unity.VisualScripting;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataManagement.Adapter
{
    internal class SaveLoadAdapter
    {
        internal string currentUserIdentification = "";
        string localFileName = "test.xml";
        string localFilePath = "D:/SaveFiles/";
        string localSavedFile = "";

        public SaveLoadAdapter() 
        {
            if (!ServerCallManager.IsConnectedToServer)
            {
                localSavedFile = localFilePath + "/" + localFileName;
            }
        }

        internal void SavePlayerData()
        {
            if (ServerCallManager.IsConnectedToServer)
            {
                // TODO: Find a way to cross reference server data and client Data

                //if (latestUserServerUpdate > UserDataBehavior.currentUserData.lastServerUpdate)
                //{
                //    // Disconnect Player if a someone else updated the savefile
                //    Application.Quit();
                //}
                //else if(latestUserServerUpdate == UserDataBehavior.currentUserData.lastServerUpdate ||
                //    latestUserServerUpdate < DateTime.UtcNow)
                //{
                //    UserDataBehavior.currentUserData.lastServerUpdate = DateTime.UtcNow;
                //}
            }

            // PLAYFAB
            PlayfabManager playfab = new PlayfabManager();
            string convertedData = JsonConvert.SerializeObject(UserDataBehavior.currentUserData);
            playfab.SavePlayerData(convertedData);
        }   

        /// <summary>
        /// gets the player data directly from the server, used for cross referencing with server data.
        /// </summary>
        /// <returns>UserData from the server</returns>
        private void getPlayerDataFromServer()
        {
            // TODO : FIND A WAY TO CROSS REFERENCE SERVER DATA AND LOCAL DATA
        }

        /// <summary>
        /// Loads Player Data and directly sets it as the current user data.
        /// </summary>
        internal void LoadPlayerData()
        {
            PlayfabManager playfab = new();
            playfab.LoadPlayerData((userData) =>
            {
                if (userData != null)
                {
                    UserDataBehavior.LoadUser(userData);
                }
                else
                {
                    UserDataBehavior.currentUserData = new UserData();

                }

                GameManager.Instance.InitializePlayer();
            });
        }
    }

}

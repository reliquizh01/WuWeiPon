using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using User.Data;
using System.IO;
using Unity.VisualScripting;
using System;

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
            XmlSerializer serializer = new XmlSerializer(typeof(UserData));

            if (ServerCallManager.IsConnectedToServer)
            {
                DateTime latestUserServerUpdate = getPlayerDataFromServer().lastServerUpdate;

                if (latestUserServerUpdate > UserDataBehavior.currentUserData.lastServerUpdate)
                {
                    // Disconnect Player if a someone else updated the savefile
                    Application.Quit();
                }
                else if(latestUserServerUpdate == UserDataBehavior.currentUserData.lastServerUpdate ||
                    latestUserServerUpdate < DateTime.UtcNow)
                {
                    UserDataBehavior.currentUserData.lastServerUpdate = DateTime.UtcNow;
                }
            }

            // TODO: Once DB Server is available, replace localFilePath with a way to connect to server
            if(!Directory.Exists(localFilePath))
            {
                Directory.CreateDirectory(localFilePath);
            }
            TextWriter writer = new StreamWriter(localSavedFile);

            serializer.Serialize(writer, UserDataBehavior.currentUserData);
            writer.Close();

        }

        private UserData getPlayerDataFromServer()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserData));

            UserData serverData = null;

            if (File.Exists(localSavedFile))
            {
                // Create a StreamReader
                TextReader reader = new StreamReader(localSavedFile);

                // Deserialize the file
                serverData = (UserData)serializer.Deserialize(reader);
    
                // Close the reader
                reader.Close();
            }
            else
            {
                serverData = CreatePlayerData();
            }

            return serverData;
        }

        private UserData CreatePlayerData()
        {
            UserData newUserData = new UserData();
            newUserData.email = AuthenticationManager.playerIdentification;

            return newUserData;
        }

        internal void LoadPlayerData()
        {
            UserDataBehavior.currentUserData = getPlayerDataFromServer();
        }
    }

}

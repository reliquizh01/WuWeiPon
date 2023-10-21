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
        string localFilePath = Application.streamingAssetsPath + "/SaveFiles/";

        public SaveLoadAdapter() 
        {
            if (!ServerCallManager.IsConnectedToServer)
            {
                localFilePath += localFileName;
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
                    // Disconnect Player
                    Application.Quit();
                }
                else if(latestUserServerUpdate == UserDataBehavior.currentUserData.lastServerUpdate)
                {
                    UserDataBehavior.currentUserData.lastServerUpdate = DateTime.UtcNow;
                }
            }

            // TODO: Once DB Server is available, replace localFilePath with a way to connect to server
            TextWriter writer = new StreamWriter(localFilePath);

            serializer.Serialize(writer, UserDataBehavior.currentUserData);
            writer.Close();

        }

        private UserData getPlayerDataFromServer()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserData));

            UserData serverData = null;

            if (File.Exists(localFilePath))
            {
                // Create a StreamReader
                TextReader reader = new StreamReader(localFilePath);

                // Deserialize the file
                serverData = (UserData)serializer.Deserialize(reader);
    
                // Close the reader
                reader.Close();
            }
            else
            {
                CreatePlayerData();
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

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

            DataContractSerializer serializer = new DataContractSerializer(typeof(UserData));
            FileStream writer = new FileStream(localSavedFile, FileMode.Create);
            serializer.WriteObject(writer, UserDataBehavior.currentUserData);
            writer.Close();
        }   

        private UserData getPlayerDataFromServer()
        {
            UserData serverData = null;

            if (File.Exists(localSavedFile))
            {
                FileStream fs = new FileStream(localSavedFile, FileMode.Open);

                XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                DataContractSerializer ser = new DataContractSerializer(typeof(UserData));

                serverData = (UserData)ser.ReadObject(reader, true);
                reader.Close();
                fs.Close();
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
            UserDataBehavior.LoadUser(getPlayerDataFromServer());
        }
    }

}

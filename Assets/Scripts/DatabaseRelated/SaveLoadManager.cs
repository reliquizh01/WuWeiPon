using DataManagement.Adapter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManagement
{
    public static class SaveLoadManager
    {
        static SaveLoadAdapter adapter = new SaveLoadAdapter();

        public static void SaveUser()
        {
            adapter.SavePlayerData();
        }

        /// <summary>
        /// Loads User, can be changed to google authentication later
        /// </summary>
        public static void LoadUser()
        {
            adapter.LoadPlayerData();
        }

        public static void ConnectAdapter()
        {
            //Obtain User email
            adapter.currentUserIdentification = AuthenticationManager.playerIdentification;

            LoadUser();
        }
    }
}

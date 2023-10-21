using DataManagement.Adapter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManagement
{
    public static class AuthenticationManager
    {
        internal static string playerIdentification;
        /// <summary>
        /// Authenticate the User by simply obtaining the player Googleplay account.
        /// </summary>
        public static void AuthenticateUserLogin()
        {
            if (ServerCallManager.IsConnectedToServer)
            {
                //TODO
                //Get Google playstore account
            }
            else
            {
                playerIdentification = "LocalUser";
            }
        }

    }
}
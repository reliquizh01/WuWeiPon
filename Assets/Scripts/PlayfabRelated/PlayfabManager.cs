using UnityEngine;
using User.Data;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class PlayfabManager
{
    private UserData userDataHolder;
    private List<Action> OnLoginSuccess = new List<Action>();
    private List<Action<UserData>> OnUserDataRetrieved = new List<Action<UserData>>();
    public void Login(Action callback)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
   
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnFail);

        OnLoginSuccess.Add(callback);
    }
   
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Login Success!");
        OnLoginSuccess.ForEach(x => x.Invoke());
        OnLoginSuccess.Clear();
    }
   
    void OnFail(PlayFabError error)
    {
        Debug.Log("Login Failed!");
        Debug.Log(error.GenerateErrorReport());
    }

    internal void SavePlayerData(string userData)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>()
            {
                { "PlayerData", userData }
            }
        };

        PlayFabClientAPI.UpdateUserData(request, playerSaveSuccess, playerSaveFail);
    }

    internal void LoadPlayerData(Action<UserData> callBack)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), playerDataReceived, playerLoadDataFail);

        OnUserDataRetrieved.Add(callBack);
    }

    void playerDataReceived(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("PlayerData"))
        {
            string data = result.Data["PlayerData"].Value;
            userDataHolder = JsonConvert.DeserializeObject<UserData>(data);

        }

        OnUserDataRetrieved.ForEach(x => x.Invoke(userDataHolder));
        OnUserDataRetrieved.Clear();

    }

    void playerLoadDataFail(PlayFabError error)
    {
        Debug.Log("Failed to Load Player Data!");
    }

    void playerSaveSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Player Saved!");
    }

    void playerSaveFail(PlayFabError error)
    {   
        Debug.Log("Player Failed to Save!");
        Debug.Log(error.GenerateErrorReport());
    }

}

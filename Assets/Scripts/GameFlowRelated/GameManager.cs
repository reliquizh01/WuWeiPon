using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User.Data;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    #region References

    public GameObject EnvironmentItems;
    public Camera mainCam;
    
    #endregion References

    [SerializeField] private string userLoggedIn;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        AuthenticationManager.AuthenticateUserLogin();
        SaveLoadManager.ConnectAdapter();

        userLoggedIn = AuthenticationManager.playerIdentification;

        CheckPlayerOnBoardingProgress();
    }

    private void CheckPlayerOnBoardingProgress()
    {
        int totalOnboardingSteps = Enum.GetNames(typeof(PlayerOnBoardingEnum)).Length;
        int playerProgress = UserDataBehavior.currentUserData.playerOnBoardingProgress.Count;

        if (totalOnboardingSteps > playerProgress)
        {
            PlayerOnBoardingTutorial tutorial = new PlayerOnBoardingTutorial(instance, (PlayerOnBoardingEnum)playerProgress);
        }
    }
}

using DataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using User.Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region References

    public GameObject EnvironmentItems;
    public Camera mainCam;
    public WeaponContainer equippedWeaponContainer;

    public GameObject TrainingUi;
    public GameObject BattleUi;

    #endregion References
    [SerializeField] internal GameStateEnum currentGameState;
    [SerializeField] private string userLoggedIn;

    private float battleCameSize = 15.0f;
    private float idleCameSize = 11.0f;
    private bool cameraTransitioning = false;
    private float camTargetSize = 0.0f;
    private float camTransitonSpeed = 20.0f;
    private List<Action> actionAfterTransiton = new List<Action>();

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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

        if(UserDataBehavior.GetPlayerEquippedWeapon() != null)
        {
            loadPlayerEquippedWeapon();
        }
        
        checkPlayerOnBoardingProgress();
    }

    public void Update()
    {
        if (cameraTransitioning)
        {
            cameraTransitionUpdate();
        }
    }
    public void SetGameState(GameStateEnum gameState, Action afterTransitionAction = null)
    {
        currentGameState = gameState;

        if(afterTransitionAction != null)
        {
            actionAfterTransiton.Add(afterTransitionAction);
        }

        switch (currentGameState)
        {
            case GameStateEnum.Idle:
                camTargetSize = idleCameSize;
                TrainingUi.SetActive(true);
                BattleUi.SetActive(false);
                break;
            case GameStateEnum.Battle:
                camTargetSize = battleCameSize;
                TrainingUi.SetActive(false);
                BattleUi.SetActive(true);
                break;
            default:
                break;
        }

        if(mainCam.orthographicSize != camTargetSize)
        {
            cameraTransitioning = true;
        }
    }
    private void loadPlayerEquippedWeapon()
    {
        GameObject weapon = PrefabManager.Instance.CreateWeaponContainer(Vector2.zero);
        equippedWeaponContainer = weapon.GetComponent<WeaponContainer>();
        equippedWeaponContainer.SetWeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
        equippedWeaponContainer.SetWeaponState(WeaponBehaviorStateEnum.Idle);
    }

    private void checkPlayerOnBoardingProgress()
    {
        int totalOnboardingSteps = Enum.GetNames(typeof(PlayerOnBoardingEnum)).Length;
        int playerProgress = UserDataBehavior.currentUserData.playerOnBoardingProgress.Count;

        if (totalOnboardingSteps > playerProgress)
        {
            PlayerOnBoardingTutorial tutorial = new PlayerOnBoardingTutorial(Instance, (PlayerOnBoardingEnum)playerProgress);
        }
    }
    
    private void cameraTransitionUpdate()
    {
        if (mainCam.orthographicSize >= camTargetSize)
        {
            mainCam.orthographicSize -= camTransitonSpeed * Time.deltaTime;

            if (mainCam.orthographicSize <= camTargetSize)
            {
                mainCam.orthographicSize = camTargetSize;
                cameraTransitioning = false;

                actionAfterTransiton.ForEach(x => x.Invoke());
                actionAfterTransiton.Clear();
            }
        }
        else
        {
            mainCam.orthographicSize += camTransitonSpeed * Time.deltaTime;

            if (mainCam.orthographicSize >= camTargetSize)
            {
                mainCam.orthographicSize = camTargetSize;
                cameraTransitioning = false;

                actionAfterTransiton.ForEach(x => x.Invoke());
                actionAfterTransiton.Clear();
            }
        }
    }
}

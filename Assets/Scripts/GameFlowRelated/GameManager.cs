using DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using User.Data;
using WeaponRelated;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region References

    public GameObject EnvironmentItems;
    public Camera mainCam;
    public WeaponContainer equippedWeaponContainer;

    public GameObject TrainingUi;
    public BattleUserInterface BattleUi;

    #endregion References

    [SerializeField] internal GameStateEnum currentGameState;
    [SerializeField] private string userLoggedIn;

    internal bool loadPlayerDataComplete = false;
    internal bool previousTransactionsInProgress = false;

    private float battleCameSize = 15.0f;
    private float prepareBattleCameraSize = 11.0f;
    private Vector2 cameraTargetPosition = new Vector2(0,0);
    private float belowThresholdBattleCamSize = 28.0f;


    private float idleCameSize = 11.0f;
    private bool cameraTransitioning = false;
    private bool cameraMovementTransitioning = false;
    private float camTargetSize = 0.0f;
    private float camTransitonSpeed = 20.0f;
    private List<Action> afterCameraTransitionActions = new List<Action>();
    private List<Action<CurrencyEnum>> updateCurrencyValue = new List<Action<CurrencyEnum>>();

    public Slider autoSpinSkill;

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
        checkPlayerAutomationSetup();

        loadPlayerDataComplete = true;

        for (int i = 0; i < Enum.GetNames(typeof(CurrencyEnum)).Length; i++)
        {
            UpdateCurrencyValues((CurrencyEnum)i);
        }

        if (!previousTransactionsInProgress)
        {
            SetGameState(GameStateEnum.Idle);
        }
    }

    private void checkPlayerAutomationSetup()
    {
        AutomationSettingsBehavior.Instance.SetupAutomation();
    }

    private void checkPlayerSkillPendingTransactions()
    {
        if(equippedWeaponContainer != null)
        {
            WeaponData weapon = UserDataBehavior.GetPlayerEquippedWeapon();

            if(weapon.skillPurchased != null)
            {
                SkillData skillEquippedInSlot = weapon.skills.FirstOrDefault(x => x.slotNumber == weapon.skillPurchased.slotNumber);
                equippedWeaponContainer.weaponSlotsContainer.skillSlots.First(x => x.slotNumber == weapon.skillPurchased.slotNumber).ContinueLastTransaction();
            }
        }
    }

    private void checkPlayerPendingWeaponCondensation()
    {
        if(equippedWeaponContainer != null)
        {
            if(UserDataBehavior.GetUserCurrentCondensation() != null){
                SoundManager.Instance.PlayBackgroundTheme(LocationEnum.NormalCondensation);
                SpiritCondensationContainer.Instance.ContinueLastTransaction();
                SetUserInterface(GameStateEnum.Condensing);
                previousTransactionsInProgress = true;
            }
        }
    }

    public void Update()
    {
        if (cameraTransitioning)
        {
            cameraTransitionUpdate();
        }
    }

    public void UpdateCurrencyValues(CurrencyEnum currency)
    {
        if (loadPlayerDataComplete)
        {
            if(updateCurrencyValue.Count > 0)
            {
                updateCurrencyValue.ForEach(x => x.Invoke(currency));
            }
        }
    }

    public void AddToUpdateCurrencyCallBacks(Action<CurrencyEnum> action)
    {
        updateCurrencyValue.Add(action);
    }

    public void RemoveFromUpdateCurrencyCallbacks(Action<CurrencyEnum> action)
    {
        updateCurrencyValue.Remove(action);
    }

    public void SetUserInterface(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.Idle:
                TrainingUi.SetActive(true);
                BattleUi.SetVisibility(false);
                if(equippedWeaponContainer != null)
                {
                    equippedWeaponContainer.ShowUI();
                }
                break;
            case GameStateEnum.Battle:
                TrainingUi.SetActive(false);
                BattleUi.SetVisibility(true);
                break;
            case GameStateEnum.Condensing:
                TrainingUi.SetActive(false);
                BattleUi.SetVisibility(false);
                if (equippedWeaponContainer != null)
                {
                    equippedWeaponContainer.HideUI();
                }
                break;
            default:
                break;
        }
    }
    public void SetGameState(GameStateEnum gameState, Action afterTransitionAction = null)
    {
        GameStateEnum prevGameState = currentGameState;

        currentGameState = gameState;

        switch (currentGameState)
        {
            case GameStateEnum.Idle:
                Time.timeScale = 1.0f;
                SoundManager.Instance.PlayBackgroundTheme(LocationEnum.GreenlandMountains);
                break;
            case GameStateEnum.Battle:
                SoundManager.Instance.PlayBackgroundTheme(LocationEnum.NormalBattle);
                break;
            case GameStateEnum.EndBattle:
                SetCameraNextSize(GameStateEnum.EndBattle);
                Time.timeScale = 0.2f;
                break;
            case GameStateEnum.Condensing:
                SoundManager.Instance.PlayBackgroundTheme(LocationEnum.NormalCondensation);
                int spiritualEssence = UserDataBehavior.GetCurrency(CurrencyEnum.spirtualEssence);

                if (spiritualEssence >= 1000)
                {
                    SpiritCondensationContainer.Instance.PrepareSpiritCondensation(1000);
                }
                else if(spiritualEssence >= 200)
                {
                    SpiritCondensationContainer.Instance.PrepareSpiritCondensation(spiritualEssence);
                }
                else
                {
                    // TODO : Inform User of Insufficient Amount of spiritual Essence
                }

                break;
            default:
                break;
        }

        SetUserInterface(currentGameState);
        CheckLastTransactions(currentGameState);
        if(afterTransitionAction != null)
        {
            afterTransitionAction.Invoke();
        }
    }

    private void CheckLastTransactions(GameStateEnum currentGameState)
    {
        switch (currentGameState)
        {
            case GameStateEnum.Idle:
                if(UserDataBehavior.GetUserCurrentCondensation() != null)
                {
                    checkPlayerPendingWeaponCondensation();
                }
                else if(UserDataBehavior.GetCurrentEquippedPurchaseSkill() != null)
                {
                    checkPlayerSkillPendingTransactions();
                }
                break;

            case GameStateEnum.Battle:
                break;
            case GameStateEnum.Condensing:
                break;
            default:
                break;
        }
    }

    public void SetCameraNextSize(GameStateEnum nextState, Action afterCameraTransitionAction = null)
    {

        switch (nextState)
        {
            case GameStateEnum.Idle:
                camTargetSize = idleCameSize;
                break;
            case GameStateEnum.Battle:
                if(Screen.width > 1050)
                {
                    camTargetSize = battleCameSize;
                }
                else
                {
                    camTargetSize = belowThresholdBattleCamSize;
                }
                break;
            case GameStateEnum.EndBattle:
                camTargetSize = prepareBattleCameraSize;
                break;
            default:
                break;
        }

        if (mainCam.orthographicSize != camTargetSize)
        {
            cameraTransitioning = true;
        }
    }

    public void SetCameraPosition(Vector2 targetPosition, Action postTransitionAction = null)
    {
        cameraTargetPosition = targetPosition;
        cameraMovementTransitioning = true;

        afterCameraTransitionActions.Add(postTransitionAction);
    }
    private void loadPlayerEquippedWeapon()
    {
        GameObject weapon = PrefabManager.Instance.CreateWeaponContainer(Vector2.zero);
        equippedWeaponContainer = weapon.GetComponent<WeaponContainer>();
        equippedWeaponContainer.SetWeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
        equippedWeaponContainer.SetWeaponState(WeaponBehaviorStateEnum.Idle);
    }

    internal void UpdateCurrentPlayer()
    {
        if(equippedWeaponContainer != null)
        {
            equippedWeaponContainer.SetWeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
        }
    }

    private void checkPlayerOnBoardingProgress()
    {
        int totalOnboardingSteps = Enum.GetNames(typeof(PlayerOnBoardingEnum)).Length;
        int playerProgress = UserDataBehavior.GetPlayerOnBoardingCount();

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
            }
        }
        else
        {
            mainCam.orthographicSize += camTransitonSpeed * Time.deltaTime;

            if (mainCam.orthographicSize >= camTargetSize)
            {
                mainCam.orthographicSize = camTargetSize;
                cameraTransitioning = false;
            }
        }

        if ((Vector2)transform.position != cameraTargetPosition && cameraTransitioning)
        {
            transform.position = Vector2.MoveTowards(transform.position, cameraTargetPosition, 12.5f);
    
            float dist = Vector2.Distance(transform.position, cameraTargetPosition);

            if (dist < 0.15f)
            {
                cameraMovementTransitioning = false;
            }
        }


        if(!cameraTransitioning && !cameraMovementTransitioning)
        {
            afterCameraTransitionActions.ForEach(x => x.Invoke());
            afterCameraTransitionActions.Clear();
        }
    }
}

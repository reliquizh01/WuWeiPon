using DataManagement;
using Identity.Randomizer;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using User.Data;
using WeaponRelated;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public BattleUserInterface userInterface;

    internal string currentBattleId = "";
    #region References

    public Transform playerPosition;
    public Transform enemyPosition;
    public Vector2 enemyWeaponIdlePosition = new Vector2(0, -17.36f);

    public WeaponContainer enemyWeapon;
    WeaponContainer playerWeapon;

    #endregion References

    public void Awake()
    {
        if (Instance == null)
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

        userInterface.setupButtons(SearchBattle);
        userInterface.setupSpeedUpButton(()=> UpdateBattleSpeed());
    }


    public void SearchBattle()
    {
        if (!ServerCallManager.IsConnectedToServer)
        {
            PrepareBattle(generateRandomEnemy());
        }
    }

    public void PrepareBattle(WeaponData enemyInformation)
    {
        GameManager.Instance.SetCameraNextSize(GameStateEnum.Battle);
        GameManager.Instance.SetUserInterface(GameStateEnum.Battle);

        playerWeapon = GameManager.Instance.equippedWeaponContainer;
        playerWeapon.MoveToPosition(playerPosition.position);
        playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.ToBattlePosition, () =>
        {
            GameManager.Instance.SetGameState(GameStateEnum.Battle, StartBattle);
        });

        enemyWeapon = PrefabManager.Instance.CreateWeaponContainer(enemyPosition.position, null).GetComponent<WeaponContainer>();
        enemyWeapon.SetWeaponData(enemyInformation);
        enemyWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);

        if (playerWeapon.dataBehavior.weaponData.weaponType == 
            enemyWeapon.dataBehavior.weaponData.weaponType)
        {
            enemyWeapon.currentWeapon.weaponSprite.color = Color.red;
        }

        
        //Setup Weapon stats and skills
        enemyWeapon.currentWeapon.SetWeaponBehavior(enemyWeapon.dataBehavior.weaponData, 2);
        List<BaseBattleSkillBehavior> enemySkills = enemyWeapon.currentWeapon.SetupWeaponSkills();

        playerWeapon.currentWeapon.SetWeaponBehavior(playerWeapon.dataBehavior.weaponData, 1); 
        List<BaseBattleSkillBehavior> playerSkills = playerWeapon.currentWeapon.SetupWeaponSkills();
        
        // Add blade action on trigger2D
        enemyWeapon.currentWeapon.AddBladeActionsForOpposingUserInterfaceUpdateOnHit(userInterface.playerInformaton.OnWeaponDamaged);
        enemyWeapon.currentWeapon.AddBladeActonForSelfUserInterfaceUpdateOnHit(userInterface.enemyInformation.OnWeaponHeal);

        playerWeapon.currentWeapon.AddBladeActionsForOpposingUserInterfaceUpdateOnHit(userInterface.enemyInformation.OnWeaponDamaged);
        playerWeapon.currentWeapon.AddBladeActonForSelfUserInterfaceUpdateOnHit(userInterface.playerInformaton.OnWeaponHeal);

        // Setup User Interface for Weapon
        userInterface.gameObject.SetActive(true);
        userInterface.playerInformaton.LoadWeaponInformation(playerWeapon.dataBehavior.weaponData,ref playerSkills);
        userInterface.enemyInformation.LoadWeaponInformation(enemyWeapon.dataBehavior.weaponData, ref enemySkills);
    }

    public void UpdateBattleSpeed(bool reset = false)
    {
        if (!reset)
        {
            Time.timeScale = (Time.timeScale == 2.0f) ? 1f : 2f;
            userInterface.speedupText.text = (Time.timeScale == 2.0f) ? "x2" : "x1";
        }
        else
        {
            Time.timeScale = 1.0f;
            userInterface.speedupText.text = "x1";
        }
    }

    public void EndBattle(WeaponBattleInformation weaponThatHasLost)
    {
        GameManager.Instance.SetCameraNextSize(GameStateEnum.Idle);

        enemyWeapon.ResetWeaponCallbacks();
        enemyWeapon.ResetWeaponPhysics();
        Destroy(enemyWeapon.gameObject);
        enemyWeapon = null;

        playerWeapon.ResetWeaponCallbacks();
        playerWeapon.ResetWeaponPhysics();

        GameManager.Instance.SetGameState(GameStateEnum.Idle, () =>
        {
            playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.ToIdlePosition);
        });

        UpdateBattleSpeed(true);
        userInterface.Reset();
        userInterface.gameObject.SetActive(false);
    }

    private void StartBattle()
    {
        userInterface.battleCounterUi.Play("BattleReadyCount", () =>
        {
            currentBattleId = RandomIdentification.RandomString(18);
            playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);
            enemyWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);

            playerWeapon.currentWeapon.weaponMovement.constantForce2d.enabled = true;
            enemyWeapon.currentWeapon.weaponMovement.constantForce2d.enabled = true;

            playerWeapon.currentWeapon.weaponMovement.AddConstantRotationForce(DirectionEnum.Left);

            enemyWeapon.currentWeapon.weaponMovement.AddConstantRotationForce(DirectionEnum.Right);

            playerWeapon.currentWeapon.weaponMovement.AddForce(DirectionEnum.Right);
            enemyWeapon.currentWeapon.weaponMovement.AddForce(DirectionEnum.Left);
        });
    }

    private WeaponData generateRandomEnemy()
    {
        // TODO
        // Make a better way to generate an enemy
        return new WeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
    }

}
using DataManagement;
using Identity.Randomizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using User.Data;
using WeaponRelated;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public BattleUserInterface userInterface;

    internal string currentBattleId = "";
    internal BattleRecordLogs currentBattleLogs;
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
        currentBattleLogs = new BattleRecordLogs();
        currentBattleLogs.battleId = currentBattleId;

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
        enemyWeapon.currentWeapon.AddBladeActionOnceBladeHitsEnemyHilt(userInterface.playerInformaton.OnWeaponDamaged);
        enemyWeapon.currentWeapon.AddWeaponActionOnChangesToSelfHealth(userInterface.enemyInformation.OnWeaponHeal);

        playerWeapon.currentWeapon.AddBladeActionOnceBladeHitsEnemyHilt(userInterface.enemyInformation.OnWeaponDamaged);
        playerWeapon.currentWeapon.AddWeaponActionOnChangesToSelfHealth(userInterface.playerInformaton.OnWeaponHeal);

        // Record Battle Logs
        playerWeapon.currentWeapon.AddBladeActionOnceBladeHitsEnemyHilt(UpdateBattleLogDamageMade);
        playerWeapon.currentWeapon.AddToHiltRecordLogs(UpdateBattleLogTotalDamageTaken);

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

    public void PrepareEndBattle(WeaponBattleInformation weaponThatHasLost)
    {
        // Camera Position, Slow Motion, and Camera Size
        GameManager.Instance.SetGameState(GameStateEnum.EndBattle);

        bool didPlayerLose = (playerWeapon.dataBehavior.weaponData.weaponId == weaponThatHasLost.curWeaponData.weaponId);
        Transform losingWeapon = (didPlayerLose) ? playerWeapon.transform : enemyWeapon.transform;


        UserDataBehavior.SaveBattleRecordLogs(new BattleRecordLogs(currentBattleLogs));


        userInterface.setupResultEndbutton(() => EndBattle(weaponThatHasLost));

        // Show Results User Interface AFTER Camera Zooming In.
        GameManager.Instance.SetCameraPosition(losingWeapon.position, ()=>
        {
            userInterface.ShowBattleResults(didPlayerLose, currentBattleLogs);
            enemyWeapon.PrepareWeaponDeath();
        });
    }

    public void EndBattle(WeaponBattleInformation weaponThatHasLost)
    {
        GameManager.Instance.SetCameraNextSize(GameStateEnum.Idle);

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
        WeaponData enemyWeapon = new WeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
        enemyWeapon.weaponId = RandomIdentification.RandomString(18);

        return enemyWeapon;
    }

    #region Battle Record Logs

    internal void UpdateBattleLogDamageMade(float totalDamageMade)
    {
        BattleTransaction tmp = new BattleTransaction();
        tmp.dealer = playerWeapon.dataBehavior.weaponData.weaponId;
        tmp.receipient = enemyWeapon.dataBehavior.weaponData.weaponId;
        tmp.amount = totalDamageMade;
        tmp.recordStats = RecordStatsEnum.DamageTaken;

        currentBattleLogs.battleTransactionLog.Add(tmp);

        UpdateBattleLogHighestOneHtDamageMade(totalDamageMade);
        UpdateBattleLogTotalDamageTaken(totalDamageMade);
    }

    internal void UpdateBattleLogHighestOneHtDamageMade(float highestOneHitDamage)
    {
        BattleTransaction highestHit = currentBattleLogs.battleTransactionLog.FirstOrDefault(x => x.recordStats == RecordStatsEnum.HighestOneHitDamage && x.amount > highestOneHitDamage);
        if (highestHit == null)
        {
            highestHit = new BattleTransaction();
            highestHit.amount = highestOneHitDamage;
            highestHit.recordStats = RecordStatsEnum.HighestOneHitDamage;
            highestHit.receipient = playerWeapon.dataBehavior.weaponData.weaponId;
            highestHit.dealer = enemyWeapon.dataBehavior.weaponData.weaponId;

            currentBattleLogs.battleTransactionLog.Add(highestHit);
        }
        else
        {
            highestHit.amount = highestOneHitDamage;
        }
    }

    internal void UpdateBattleLogTotalDamageTaken(float totalDamageReceived)
    {
        BattleTransaction totalDamage = currentBattleLogs.battleTransactionLog.FirstOrDefault(x => x.recordStats == RecordStatsEnum.TotalDamage);
        if (totalDamage == null)
        {
            totalDamage = new BattleTransaction();
            totalDamage.receipient = playerWeapon.dataBehavior.weaponData.weaponId;
            totalDamage.dealer = enemyWeapon.dataBehavior.weaponData.weaponId;
            totalDamage.amount = totalDamageReceived;
            totalDamage.recordStats = RecordStatsEnum.TotalDamage;

            currentBattleLogs.battleTransactionLog.Add(totalDamage);
        }
        else
        {
            totalDamage.amount += totalDamageReceived;
        }

    }

    #endregion Battle Record Logs

}
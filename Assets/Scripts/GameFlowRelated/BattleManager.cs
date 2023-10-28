using DataManagement;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using User.Data;
using WeaponRelated;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public BattleUserInterface userInterface;

    #region References

    public Transform playerPosition;
    public Transform enemyPosition;
    
    WeaponContainer enemyWeapon;
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
        playerWeapon = GameManager.Instance.equippedWeaponContainer;
        playerWeapon.MoveToPosition(playerPosition.position);
        playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.ToBattlePosition);

        enemyWeapon = PrefabManager.Instance.CreateWeaponContainer(enemyPosition.position).GetComponent<WeaponContainer>();
        enemyWeapon.SetWeaponData(enemyInformation);

        if(playerWeapon.dataBehavior.weaponData.weaponType == 
            enemyWeapon.dataBehavior.weaponData.weaponType)
        {
            enemyWeapon.currentWeapon.weaponSprite.color = Color.red;
        }

        enemyWeapon.currentWeapon.AddBladeAction(userInterface.playerInformaton.OnWeaponDamaged);
        enemyWeapon.currentWeapon.SetWeaponBehavior(enemyWeapon.dataBehavior.weaponData); 

        playerWeapon.currentWeapon.AddBladeAction(userInterface.enemyInformation.OnWeaponDamaged);
        playerWeapon.currentWeapon.SetWeaponBehavior(enemyWeapon.dataBehavior.weaponData); 

        // Setup User Interface for Weapon
        userInterface.gameObject.SetActive(true);
        userInterface.playerInformaton.LoadWeaponInformation(playerWeapon.dataBehavior.weaponData);
        userInterface.enemyInformation.LoadWeaponInformation(enemyWeapon.dataBehavior.weaponData);

        GameManager.Instance.SetGameState(GameStateEnum.Battle, StartBattle);
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
        enemyWeapon.ResetWeaponCallbacks();
        enemyWeapon.ResetWeaponPhysics();
        Destroy(enemyWeapon.gameObject);

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
        Debug.Log("Battle Starts");
        playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);
        enemyWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);

        playerWeapon.currentWeapon.weaponMovement.AddConstantRotationForce(DirectionEnum.Right);
        playerWeapon.currentWeapon.weaponMovement.AddTorqueForce(DirectionEnum.Right);

        enemyWeapon.currentWeapon.weaponMovement.AddConstantRotationForce(DirectionEnum.Left);
        enemyWeapon.currentWeapon.weaponMovement.AddTorqueForce(DirectionEnum.Left);

        StartCoroutine(EnergizeBattle());
    }

    private IEnumerator EnergizeBattle()
    {
        yield return new WaitForSeconds(2);

        playerWeapon.currentWeapon.weaponMovement.AddForce(DirectionEnum.Right);
        enemyWeapon.currentWeapon.weaponMovement.AddForce(DirectionEnum.Left);

        playerWeapon.currentWeapon.weaponMovement.constantForce2d.enabled = true;
        enemyWeapon.currentWeapon.weaponMovement.constantForce2d.enabled = true;
    }

    private WeaponData generateRandomEnemy()
    {
        // TODO
        // Make a better way to generate an enemy
        return new WeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
    }

}
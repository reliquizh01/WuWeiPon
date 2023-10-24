using DataManagement;
using System;
using System.Collections;
using UnityEngine;
using User.Data;

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
        enemyWeapon = PrefabManager.Instance.CreateWeaponContainer(enemyPosition.position).GetComponent<WeaponContainer>();
        enemyWeapon.SetWeaponData(enemyInformation);

        playerWeapon = GameManager.Instance.equippedWeaponContainer;
        playerWeapon.MoveToPosition(playerPosition.position);
        playerWeapon.SetWeaponState(WeaponBehaviorStateEnum.ToBattlePosition);

        GameManager.Instance.SetGameState(GameStateEnum.Battle, StartBattle);

        // Setup User Interface for Weapon
        userInterface.playerInformaton.LoadWeaponInformation(playerWeapon.dataBehavior.weaponData);
        userInterface.enemyInformation.LoadWeaponInformation(enemyWeapon.dataBehavior.weaponData);

        enemyWeapon.currentWeapon.AddBladeAction(userInterface.playerInformaton.OnWeaponDamaged);
        playerWeapon.currentWeapon.AddBladeAction(userInterface.enemyInformation.OnWeaponDamaged);
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
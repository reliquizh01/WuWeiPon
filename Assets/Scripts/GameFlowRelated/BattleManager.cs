using DataManagement;
using System;
using UnityEngine;
using UnityEngine.UI;
using User.Data;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    #region References

    public Button FightButton;

    public Transform playerPosition;
    public Transform enemyPosition;
    [SerializeField]WeaponBehavior enemyWeapon;

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

        setupButtons();
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
        enemyWeapon = PrefabManager.Instance.CreateWeaponContainer(enemyPosition.position).GetComponent<WeaponBehavior>();
        enemyWeapon.SetWeaponData(enemyInformation);

        GameManager.Instance.equippedWeaponContainer.MoveToPosition(playerPosition.position);
        GameManager.Instance.equippedWeaponContainer.SetWeaponState(WeaponBehaviorStateEnum.ToBattlePosition);


        GameManager.Instance.SetGameState(GameStateEnum.Battle, StartBattle);
    }

    private void StartBattle()
    {
        GameManager.Instance.equippedWeaponContainer.SetWeaponState(WeaponBehaviorStateEnum.Battle);
        enemyWeapon.SetWeaponState(WeaponBehaviorStateEnum.Battle);

        GameManager.Instance.equippedWeaponContainer.currentWeapon.AddForce(DirectionEnum.Right);
        GameManager.Instance.equippedWeaponContainer.currentWeapon.AddRotationalForce(DirectionEnum.Right);

        enemyWeapon.currentWeapon.AddForce(DirectionEnum.Left);
        enemyWeapon.currentWeapon.AddRotationalForce(DirectionEnum.Left);
    }

    private WeaponData generateRandomEnemy()
    {
        // TODO
        // Make a better way to generate an enemy
        return new WeaponData(UserDataBehavior.GetPlayerEquippedWeapon());
    }

    private void setupButtons()
    {
        FightButton.onClick.RemoveAllListeners();

        FightButton.onClick.AddListener(SearchBattle);
    }
}
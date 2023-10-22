using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User.Data;

public class PrefabManager : MonoBehaviour
{
    #region Constructor

    public static PrefabManager Instance;

    #endregion Constructor

    #region References

    [Header("WeaponPrefabs")]
    public GameObject weaponPrefab;
    public GameObject treasureChest;
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

    }

    public GameObject CreateWeaponContainer(Vector2 instantiateToPosition, Transform parent = null)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, parent);


        if (parent == null)
        {
            newWeapon.transform.position = instantiateToPosition;
        }
        else
        {
            newWeapon.transform.localPosition = instantiateToPosition;
        }

        return newWeapon;
    }

    /// <summary>
    /// Creates a TreasureChest GameObject to the scene.
    /// </summary>
    /// <param name="instantiateToPosition">If parent is provided, position is set as localPosition, else set as worldPosition.</param>
    /// <param name="parent">Sets a transform object as a parent</param>
    public GameObject CreateTreasureChest(Vector2 instantiateToPosition, Transform parent = null)
    {
        GameObject newChest = Instantiate(treasureChest, parent);

        if(parent == null)
        {
            newChest.transform.position = instantiateToPosition;
        }
        else
        {
            newChest.transform.localPosition = instantiateToPosition;
        }

        return newChest;
    }
}
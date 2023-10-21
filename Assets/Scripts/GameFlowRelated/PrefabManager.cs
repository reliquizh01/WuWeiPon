using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User.Data;

public class PrefabManager : MonoBehaviour
{
    #region Constructor

    public static PrefabManager instance;

    #endregion Constructor

    #region References

    [Header("WeaponPrefabs")]
    public GameObject weaponPrefab;

    #endregion References

    public void Awake()
    {
        if (instance == null)
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

    }

}
﻿using System;
using Equipment.RefactoringSlot;
using Portrait;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterSlotR[] TeamSlots = new CharacterSlotR[3];
    public GameObject[] Characters = new GameObject[3];
    public PortraitItem[] PortraitItems = new PortraitItem[3];
    public CharacterStat[] CharacterStats = new CharacterStat[3];

    public bool[] IsAlive = new bool[3];
    
    public bool BossKill { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < IsAlive.Length; i++)
        {
            IsAlive[i] = true;
        }
       
    }
    public void Expedition()
    {
        for (int i = 0; i < IsAlive.Length; i++)
        {
            IsAlive[i] = true;
        }
    }

    public void SetBossKill()
    {
        BossKill = true;
    }

    public void ResetBossKill()
    {
        BossKill = false;
    }
    
    
}

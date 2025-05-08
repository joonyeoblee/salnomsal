using System.Collections.Generic;
using Equipment.RefactoringSlot;
using Portrait;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterSlotR[] TeamSlots = new CharacterSlotR[3];
    public GameObject[] Characters = new GameObject[3];
// GameManager 내부
    public CharacterStat[] CharacterStats = new CharacterStat[3];
    public List<PortraitItem> PortraitItems = new List<PortraitItem>();
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
    }
    
    public void Expedition()
    {
        // CharacterStats.Clear();
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

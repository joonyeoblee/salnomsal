using System.Collections.Generic;
using Portrait;
using Team;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TeamSlot[] TeamSlots = new TeamSlot[3];
    public List<string> Teams = new List<string>();
    public List<GameObject> Characters = new List<GameObject>();
// GameManager 내부
    public List<CharacterStat> CharacterStats = new List<CharacterStat>();

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
        Characters.Clear();
        CharacterStats.Clear(); // ★ 스탯도 초기화

        foreach (TeamSlot slot in TeamSlots)
        {
            Teams.Add(slot.SaveKey);

            if (slot.currentCharacterPortrait != null)
            {
                PortraitItem _portraitItem = slot.currentCharacterPortrait.GetComponent<PortraitItem>();
                GameObject character = _portraitItem.portrait.Character;

    
                Characters.Add(character);
                CharacterStats.Add(_portraitItem.SaveData.CharacterStat);
            }
            else
            {
                Characters.Add(null);
                CharacterStats.Add(null);
            }
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

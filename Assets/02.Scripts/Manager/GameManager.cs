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
    public List<PortraitItem> PortraitItems = new List<PortraitItem>();
    public bool BossKill { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("🟢 GameManager 인스턴스 등록됨");
        }
        else if (Instance != this)
        {
            Debug.LogWarning("⚠️ 중복 GameManager 감지됨. 파괴됨: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Expedition()
    {
        CharacterStats.Clear();
        Teams.Clear();

        for (var i = 0; i < TeamSlots.Length; i++)
        {
            TeamSlot slot = TeamSlots[i];
            Teams.Add(slot.SaveKey);

            if (slot.currentCharacterPortrait != null)
            {
                PortraitItem portraitItem = slot.currentCharacterPortrait.GetComponent<PortraitItem>();
                if (PortraitItems.Count <= i)
                {
                    PortraitItems.Add(portraitItem);
                }
                else
                {
                    PortraitItems[i] = portraitItem;
                }

                CharacterStats.Add(portraitItem.SaveData.CharacterStat);
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

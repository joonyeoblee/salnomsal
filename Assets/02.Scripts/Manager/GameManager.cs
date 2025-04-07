using System.Collections.Generic;
using Portrait;
using Team;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<TeamSlot> TeamSlots = new List<TeamSlot>(3);
    public List<GameObject> Characters = new List<GameObject>(3);
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Expedition()
    {
        Characters.Clear(); // 기존 캐릭터 리스트 초기화

        foreach (TeamSlot slot in TeamSlots)
        {
            if (slot.currentCharacterPortrait != null)
            {
                Characters.Add(slot.currentCharacterPortrait.GetComponent<PortraitItem>().portrait.Character);
                Debug.Log($"출정 캐릭터 추가됨: {slot.currentCharacterPortrait.name}");
            } else
            {
                Characters.Add(null); // 슬롯이 비어있으면 null로 채움
                Debug.Log("출정 슬롯이 비어 있음");
            }
        }

        Debug.Log($"총 출정 캐릭터 수: {Characters.Count}");
    }
    
}

using Equipment.RefactoringSlot;
using Portrait;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_ChestInventory : MonoBehaviour
{
    public Image[] Portraits;
    public TMP_Text[] Statas;

    public EquimentSlotR[] Weapon;
    public EquimentSlotR[] Armor;
    
    public static UI_ChestInventory Instance;
    private GameObject[] _PortraitItems
    {
        get
        {
            return GameManager.Instance.Characters;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // else if (Instance != this)
        // {
        //     Destroy(gameObject);
        // }
    }

    public void Initialize(PlayableCharacter character, int index)
    {
        Debug.Log("UI_ChestInventory::Initialize" + index + "::" + character);
        if (character == null)
        {
            Portraits[index].gameObject.SetActive(false);
            return;
        }
        Portraits[character.Index].GetComponent<Image>().sprite = character.GetComponent<PlayableIcons>().Portrait;
    }

    public void InitCharacterStats(CharacterStat character, int index)
    {
        if (character == null)
        {
            Statas[index].SetText("");
            return;
        }
        Statas[index].text = $"최대 체력 : {(int)character.MaxHealth}\n최대 마나 : {character.MaxMana}\n공격력 : {(int)character.AttackPower}\n속도 : {character.Speed}\n";

    }
    
}

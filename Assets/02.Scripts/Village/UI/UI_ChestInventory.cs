using System;
using System.Collections;
using System.Collections.Generic;
using Portrait;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChestInventory : MonoBehaviour
{
    public Image[] Portraits;
    public TMP_Text[] Statas;
    
    private List<PortraitItem> _PortraitItems => GameManager.Instance.PortraitItems;
    private List<PlayableCharacter> _Characters => CombatManager.Instance.PlayableCharacter;
    
    
    void Start()
    {
        int count = Mathf.Min(
            _PortraitItems.Count,
            _Characters.Count,
            Portraits.Length,
            Statas.Length
        );

        for (int i = 0; i < count; i++)
        {
            PortraitItem portraitItem = _PortraitItems[i];
            PortraitSO portrait = portraitItem.portrait;

            Portraits[i].sprite = portrait.Icon;
            Statas[i].text = $"HP :  {(int)_Characters[i].MaxHealth} \nCost : {(int)_Characters[i].MaxCost}\n공격력 : {(int)_Characters[i].AttackPower}\n속도 : {_Characters[i].CurrentSpeed}\n";
        }
    }


    
}

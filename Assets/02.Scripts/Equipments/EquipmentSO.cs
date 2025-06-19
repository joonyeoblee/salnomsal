using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public enum EquipmentType
    {
        Weapon,
        Armor
    }
    
    [CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
    public class EquipmentSO : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public Sprite BorderSprite;
        public EquipmentType Type; 

        [Header("기본 능력치 범위")] public List<StatRange> BaseStatRanges;

        [Header("패시브 효과")] public List<PassiveEffect> PassiveEffects;
    }
}
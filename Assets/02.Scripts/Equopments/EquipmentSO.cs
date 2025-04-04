using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Equipment
{
    [CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
    public class EquipmentSO : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public EquipmentType Type; // ✅ 외부 enum 사용
        public Rarity Rarity;

        [Header("기본 능력치 범위")] public List<StatRange> BaseStatRanges;

        [Header("패시브 효과")] public List<PassiveEffect> PassiveEffects;
    }
}
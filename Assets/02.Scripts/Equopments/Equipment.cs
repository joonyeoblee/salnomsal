using System;
using UnityEngine;

namespace Equipment
{
    public enum EquipmentType
    {
        Weapon,
        Armor
    }

    [Serializable]
    public class Equipment
    {
        public EquipmentSO EquipmentSO;
        public Sprite Icon;
    }
}
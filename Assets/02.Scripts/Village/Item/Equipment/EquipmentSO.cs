using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
    public class EquipmentSO : ScriptableObject
    {
        public string itemName;
        public EquipmentType type;
        public int attack;
        public int defense;
    }
}

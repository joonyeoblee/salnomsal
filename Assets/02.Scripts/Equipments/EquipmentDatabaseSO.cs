using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Scriptable Objects/EquipmentDatabase")]
    public class EquipmentDatabase : ScriptableObject
    {
        public List<EquipmentSO> equipmentList;

        private Dictionary<string, EquipmentSO> _lookup;

        private void OnEnable()
        {
            _lookup = new Dictionary<string, EquipmentSO>();
            foreach (EquipmentSO so in equipmentList)
                if (!_lookup.ContainsKey(so.ItemName))
                {
                    _lookup.Add(so.ItemName, so);
                }
                else
                {
                    Debug.LogWarning($"중복된 이름 발견: {so.ItemName} (데이터베이스 내에서 이름은 고유해야 합니다)");
                }
        }

        public EquipmentSO GetEquipmentByName(string name)
        {
            if (_lookup.TryGetValue(name, out EquipmentSO so))
            {
                return so;
            }

            Debug.LogError($"[EquipmentDatabase] 이름 '{name}' 에 해당하는 SO를 찾을 수 없습니다.");
            return null;
        }
    }
}
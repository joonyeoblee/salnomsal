using UnityEngine;

namespace Equipment
{
    public class EquipmentGenerator : MonoBehaviour
    {
        [SerializeField] private EquipmentSO _data;
        public EquipmentInstance EquipmentInstance { get; private set; }

        void Awake()
        {
            EquipmentInstance = EquipmentFactory.Create(_data, _data.Rarity);

            if (EquipmentInstance != null)
            {
                Debug.Log($"템플릿에 등록된 패시브 수: {_data.PassiveEffects.Count}");
                
                Debug.Log($"[생성된 장비] {_data.ItemName} ({_data.Rarity})");

                foreach (StatModifier stat in EquipmentInstance.BaseStats)
                {
                    float decimalPart = stat.Value - Mathf.Floor(stat.Value);
                    int displayValue = (decimalPart >= 0.5f) ? Mathf.CeilToInt(stat.Value) : Mathf.FloorToInt(stat.Value);
                    Debug.Log($"→ 능력치: {stat.StatType} +{displayValue}");
                }
                foreach (AppliedPassiveEffect passive in EquipmentInstance.AppliedPassives)
                {
                    float decimalPart = passive.Value - Mathf.Floor(passive.Value);
                    int displayValue = (decimalPart >= 0.5f) ? Mathf.CeilToInt(passive.Value) : Mathf.FloorToInt(passive.Value);
                    Debug.Log($"→ 패시브: {passive.PassiveType} +{displayValue}");
                }
            }
            else
            {
                Debug.LogWarning("장비 생성 실패!");
            }
        }
    }
}
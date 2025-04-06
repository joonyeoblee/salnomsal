using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Equipment
{
    public class EquipmentGenerator : MonoBehaviour
    {
        [SerializeField] private EquipmentSO _data;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _borderImage;
        public EquipmentInstance EquipmentInstance { get; private set; }

        void Awake()
        {
            EquipmentInstance = EquipmentFactory.Create(_data, _data.Rarity);

            // 1. 생성된 아이템에 EquipmentDragItem 컴포넌트를 가져옴
            EquipmentDragItem dragItem = GetComponent<EquipmentDragItem>();

            // 2. 초기화 호출
            if (dragItem != null && EquipmentInstance != null)
            {
                dragItem.Initialize(EquipmentInstance);
            }
            
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
                
                _iconImage.sprite = EquipmentInstance.Template.Icon;
                _borderImage.sprite = EquipmentInstance.Template.BorderSprite;

                SetBorderColor(EquipmentInstance.Template.Rarity);
            }
            else
            {
                Debug.LogWarning("장비 생성 실패!");
            }
        }
        
        private void SetBorderColor(Rarity rarity)
        {
            Color color = rarity switch
            {
                Rarity.Common    => Color.gray,
                Rarity.Rare      => new Color(0.2f, 0.6f, 1f),    // 파란빛
                Rarity.Epic      => new Color(0.6f, 0f, 1f),      // 보라빛
                Rarity.Legendary => new Color(1f, 0.8f, 0f),      // 금색
            };

            _borderImage.color = color;
        }
    }
}
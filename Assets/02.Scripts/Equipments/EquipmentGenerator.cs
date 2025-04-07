using UnityEngine;
using UnityEngine.UI;

namespace Equipment
{
    public class EquipmentGenerator : MonoBehaviour
    {
        public string ItemId;
        readonly string SAVE_KEY = "Item_";
        string key => SAVE_KEY + ItemId;
        EquipmentSaveData _saveData;
        [SerializeField] EquipmentSO _equipmentSo;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _borderImage;
        public EquipmentInstance EquipmentInstance { get; private set; }

        void Start()
        {
            Init(ItemId);
        }
        public void Init(string id)
        {
            ItemId = id;
            _saveData = Load();

            if (_saveData == null)
            {
                EquipmentInstance = EquipmentFactory.Create(_equipmentSo);
                _saveData = new EquipmentSaveData(ItemId, EquipmentInstance);
                Save();
            } else
            {
                Debug.Log("불러옴");
                EquipmentInstance = _saveData.ToInstance(_equipmentSo);
            }

            // 1. 생성된 아이템에 EquipmentDragItem 컴포넌트를 가져옴
            EquipmentDragItem dragItem = GetComponent<EquipmentDragItem>();

            // 2. 초기화 호출
            if (dragItem != null && EquipmentInstance != null)
            {
                dragItem.Initialize(EquipmentInstance);
            }
            
            if (EquipmentInstance != null)
            {
                Debug.Log($"[생성된 장비] {_equipmentSo.ItemName})");

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

                SetBorderColor(EquipmentInstance.Rarity);
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

        void Save()
        {
            string jsonData = JsonUtility.ToJson(_saveData);
            PlayerPrefs.SetString(key, jsonData);
        }

        EquipmentSaveData Load()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string data = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<EquipmentSaveData>(data);
            }
            return null;
        }
    }
}
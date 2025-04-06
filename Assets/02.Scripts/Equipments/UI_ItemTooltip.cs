using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace Equipment
{
    public class UI_ItemTooltip : MonoBehaviour
    {
        public static UI_ItemTooltip Instance;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _detailText;
        [SerializeField] private RectTransform _panel;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        
        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
            
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out anchoredPos);

            _rectTransform.anchoredPosition = anchoredPos + new Vector2(0f, -200f);
        }

        
        public void Show(EquipmentInstance equipment)
        {
            if (equipment == null) return;

            gameObject.SetActive(true);
            _nameText.text = equipment.Template.ItemName;

            string text = "";
            foreach (var stat in equipment.BaseStats)
                text += $"{stat.StatType}: {Mathf.RoundToInt(stat.Value)}\n";

            foreach (var passive in equipment.AppliedPassives)
                text += $"<color=yellow>{passive.PassiveType}: +{Mathf.RoundToInt(passive.Value)}</color>\n";

            _detailText.text = text;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
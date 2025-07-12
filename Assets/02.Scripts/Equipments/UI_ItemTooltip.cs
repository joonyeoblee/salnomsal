using TMPro;
using UnityEngine;

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
            Vector2 mousePos = Input.mousePosition;

            // 화면 중심 기준 좌/우 판단
            var isLeft = mousePos.x < Screen.width / 2f;
            Vector2 offset = isLeft ? new Vector2(20f, -20f) : new Vector2(-_panel.rect.width - 20f, -20f);

            // 마우스 → 로컬 좌표 (카메라 모드니까 worldCamera 넣어줌)
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                mousePos,
                _canvas.worldCamera,
                out localPoint
            );

            _rectTransform.anchoredPosition = localPoint + offset;
        }

        
        public void Show(EquipmentInstance equipment)
        {
            if (equipment == null) return;

            gameObject.SetActive(true);
            // _nameText.text = equipment.Template.ItemName;

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
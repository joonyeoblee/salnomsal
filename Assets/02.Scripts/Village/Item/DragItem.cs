using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Equipment equipment;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private Transform originalParent;
        private Vector2 originalPosition;

        public bool IsInSlot { get; set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsInSlot)
            {
                return;
            }

            originalParent = transform.parent;
            originalPosition = rectTransform.anchoredPosition;

            transform.SetParent(canvas.transform);
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            Slot targetSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();

            if (targetSlot == null)
            {
                // 슬롯이 아닌 곳 → 삭제
                Debug.Log("슬롯 외부로 드롭됨 → 삭제");
                // Destroy(gameObject);
            }

            IsInSlot = true;
            // 슬롯에 잘 드롭됨 → 처리 생략 (OnDrop이 자동으로 실행됨)
        }

        public void StartManualDrag()
        {
            originalParent = transform;
            originalPosition = GetComponent<RectTransform>().anchoredPosition;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsLastSibling(); // 맨 앞으로
        }
    }
}
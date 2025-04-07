using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jun.Map
{
    public class UI_MapNode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image iconImage;
        public TMP_Text label;

        public MapNode Source { get; private set; } // 연결된 실제 노드
        bool interactable;
        Color originalColor; // 원래 색상 저장용

        public void Init(Vector2 position, string labelText, Color color, MapNode source)
        {
            GetComponent<RectTransform>().anchoredPosition = position;
            iconImage.color = color;
            originalColor = color; // 초기 색상 저장
            label.text = labelText;
            Source = source;
            SetInteractable(false); // 기본은 비활성화
        }


        public void SetInteractable(bool value)
        {
            interactable = value;
            iconImage.raycastTarget = value;
            iconImage.color = value ? originalColor : Color.gray; // true면 원래 색, false면 회색
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("나 클릭됌");
            if (!interactable) return;

            MapManager.instance.SetCurrentNode(Source);
            Debug.Log($"노드 선택: {Source.X}, {Source.Y}");

            // TODO: 맵 선택 적 소환 컴뱃
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}

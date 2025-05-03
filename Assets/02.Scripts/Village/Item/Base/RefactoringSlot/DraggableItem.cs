using System;
using Portrait;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ItemData : MonoBehaviour
{
    public string Id;
    public string Name;

    public ItemType CurrentItemType;

    public ItemData(string id, string name, ItemType itemType)
    {
        Id = id;
        Name = name;
        CurrentItemType = itemType;
    }
}

public enum ItemType
{
    Portrait,
    Equipment
}

[Serializable]
public class PortraitItemData
{
    public ItemData ItemData;
    public string PortraitName;
    public CharacterStat CharacterStat;
    public int ClearCount;

    public PortraitItemData(ItemData itemData, string portraitName, CharacterStat characterStat, int clearCount)
    {
        ItemData = itemData;
        PortraitName = portraitName;
        CharacterStat = characterStat;
        ClearCount = clearCount;
    }
}
public abstract class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    // 위치를 위한 데이터들
    protected RectTransform _rectTransform;
    protected Canvas _canvas;
    protected CanvasGroup _canvasGroup;

    protected Transform _originalParent;
    protected Vector2 _originalPosition;
    public SlotR MyParent;
    public SlotR OldParent;
    public bool IsInSlot;

    // 개인 정보
    protected string id;
    [SerializeField] protected Image _iconImage;
    protected ItemData _itemData;
    public PortraitItemData SaveData;
    public string Key => "Character_" + id;
    
    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        _iconImage = GetComponent<Image>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    { 
        _originalParent = transform.parent;
        OldParent = MyParent;
        _originalPosition = _rectTransform.anchoredPosition;

        Debug.Log("마우스 눌림");
        transform.SetParent(_canvas.transform);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
    
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        
        // 드롭된 슬롯 판단
        SlotR targetSlot = eventData.pointerEnter?.GetComponentInParent<SlotR>();
        Debug.Log(targetSlot);
        //
        // if (targetSlot != null)
        // {
        //     // 슬롯이 유효하면 슬롯이 처리하도록 위임 (슬롯에서 SetItem 또는 ChangeSlot 실행됨)
        //     return;
        // }
        //
        // 슬롯에 드롭하지 않았을 경우 원래대로 복귀
        ReturnToOriginalParent();
    }
    void ReturnToOriginalParent()
    {
        MyParent = OldParent;

        if (MyParent != null)
        {
            MyParent.SetItem(this); // 슬롯의 currentCharacterPortrait도 복구됨
            transform.SetParent(MyParent.transform); // 부모 복구
            transform.localPosition = MyParent.DraggedSlot.localPosition; // 위치 복구
            IsInSlot = true;
        } else
        {
            // 슬롯이 아닌 곳에서 드래그된 경우 처리
            transform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = _originalPosition;
            IsInSlot = false;
        }
    }
}
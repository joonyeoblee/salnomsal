using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotR : MonoBehaviour, IDropHandler
{
    [SerializeField] DraggableItem _myDraggableItem;
    
    public RectTransform DraggedSlot;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();
        droppedRect.SetParent(this.transform);
        droppedRect.anchoredPosition = Vector2.zero;
    }
    
    public void SetItem(DraggableItem myDraggableItem)
    {
        _myDraggableItem = myDraggableItem;
    }
}
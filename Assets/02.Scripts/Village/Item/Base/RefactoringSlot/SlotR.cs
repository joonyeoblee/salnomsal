using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public abstract class ItemData : MonoBehaviour
{
    public string Id;
    public string Name;
    public string Type;
    public int Quantity;
    
    public ItemType CurrentItemType;
    public ItemLocation CurrentLocation;
}
public enum ItemLocation
{
    Storage,
    WorldSpawn,
    Equipped
}

public enum ItemType
{
    Portrait,
    Equipment
}
public abstract class SlotR : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [SerializeField] DraggableItem _myDraggableItem;
    
    public RectTransform DraggedSlot;
    [SerializeField] ItemLocation _location;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();
        droppedRect.SetParent(this.transform);
        droppedRect.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void SetItem(DraggableItem myDraggableItem)
    {
        _myDraggableItem = myDraggableItem;
    }
}
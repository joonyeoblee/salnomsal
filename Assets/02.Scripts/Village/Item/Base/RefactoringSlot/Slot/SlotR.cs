using System;
using Portrait;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class SlotR : MonoBehaviour, IDropHandler
{

    [Serializable]
    public class CharacterSlotItemData
    {
        public string SaveKey;
        public string MyDraggableItemID;

    }
    [SerializeField] protected DraggableItem _myDraggableItem;
    
    public RectTransform DraggedSlot;

    public string MyDraggableItemID;
    public string SlotItemID;

    public GameObject ItemPrefab;
    public string SaveKey
    {
        get
        {
            return "CharacterSlot_" + SlotItemID + transform.GetSiblingIndex();
        }
    }
    protected virtual void Start()
    {
        Load();
    }
    public void OnDrop(PointerEventData eventData)
    {

        RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();
        droppedRect.SetParent(this.transform);
        droppedRect.anchoredPosition = Vector2.zero;
    }

    public virtual void SetItem(DraggableItem myDraggableItem)
    {
        _myDraggableItem = myDraggableItem;
        Debug.Log(_myDraggableItem.Id);
        // Save();
    }
    public virtual void DeleteItem()
    {
        _myDraggableItem = null;
        // Save();
    }

    public void Save()
    {
        // if (_myDraggableItem == null) return;

        CharacterSlotItemData slotItemData = new CharacterSlotItemData();

        slotItemData.MyDraggableItemID = _myDraggableItem.Id;
        slotItemData.SaveKey = SaveKey;
        string data = JsonUtility.ToJson(slotItemData);

        // 내 키와 아이템 Id 묶어서 저장
        PlayerPrefs.SetString(SaveKey, data);
        PlayerPrefs.Save();
        Debug.Log("Save" + SaveKey);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            CharacterSlotItemData loadSlotItemData = JsonUtility.FromJson<CharacterSlotItemData>(json);

            GameObject newItem = Instantiate(ItemPrefab, transform);
            newItem.GetComponent<PortraitItem>().Load(loadSlotItemData.MyDraggableItemID);
            Debug.Log(loadSlotItemData.MyDraggableItemID);
            _myDraggableItem = newItem.GetComponent<DraggableItem>();
            _myDraggableItem.MyParent = this;

        }
    }
}
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
    public DraggableItem MyDraggableItem;
    
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
    public void ChangeSlot(PortraitItem newPortraitItem)
    {
        // 현재 슬롯에 있는 아이템
        GameObject previousItem = MyDraggableItem.gameObject;

        // 현재 슬롯의 이전 아이템 정보
        DraggableItem previousPortrait = MyDraggableItem;
        SlotR oldSlotOfNewPortrait = newPortraitItem.MyParent;

        oldSlotOfNewPortrait.SetItem(previousPortrait);
        previousPortrait.MyParent = oldSlotOfNewPortrait;
        oldSlotOfNewPortrait.Save();

        // 새 아이템을 현재 슬롯에 설정
        newPortraitItem.MyParent = this;
        SetItem(newPortraitItem);
        Save();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        DraggableItem droppedItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (droppedItem == null) return;

        SlotR oldSlot = droppedItem.MyParent;

        if (MyDraggableItem != null)
        {
            DraggableItem tempItem = MyDraggableItem;

            // 현재 아이템을 이전 슬롯으로 이동
            if (oldSlot != null && oldSlot != this)
            {
                oldSlot.SetItem(tempItem);
                tempItem.transform.SetParent(oldSlot.transform);
                tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                tempItem.MyParent = oldSlot; // ✅ 중요!
            }
            else
            {
                tempItem.transform.SetParent(null);
                tempItem.MyParent = null;
            }
        }

        // 새 아이템 이 슬롯에 설정
        SetItem(droppedItem);
        droppedItem.transform.SetParent(transform);
        droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        droppedItem.MyParent = this; // ✅ 여기도 확실하게!

        Save();
        oldSlot?.Save();
    }



    public virtual void SetItem(DraggableItem myDraggableItem)
    {
        MyDraggableItem = myDraggableItem;
        MyDraggableItem.MyParent = this;
        Debug.Log(MyDraggableItem.Id);
        // Save();
    }
    public virtual void DeleteItem(bool destroyObject = false)
    {
        if (MyDraggableItem != null)
        {
            if (destroyObject)
            {
                Destroy(MyDraggableItem.gameObject); // 드래그 중이 아닐 때만 삭제
            }
        
            MyDraggableItem = null;
            Debug.Log($"[Slot {SaveKey}] 아이템 제거됨.");
        }

        if (PlayerPrefs.HasKey(SaveKey))
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            Debug.Log($"[Slot {SaveKey}] 저장 데이터 삭제됨.");
        }
    }

    public void Save()
    {
        // if (_myDraggableItem == null) return;

        CharacterSlotItemData slotItemData = new CharacterSlotItemData();

        slotItemData.MyDraggableItemID = MyDraggableItem.Id;
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
            MyDraggableItem = newItem.GetComponent<DraggableItem>();
            MyDraggableItem.MyParent = this;

        }
    }
}
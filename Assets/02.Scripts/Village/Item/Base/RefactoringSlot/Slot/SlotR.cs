﻿using System;
using Equipment;
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
    public string SaveKey;
        
    
    public virtual void OnDrop(PointerEventData eventData)
    {
        DraggableItem droppedItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (droppedItem == null) return;

        SlotR oldSlot = droppedItem.MyParent;

        if (MyDraggableItem != null) // 아이템이 있음
        {
            DraggableItem tempItem = MyDraggableItem;

            oldSlot.SetItem(tempItem);
            tempItem.transform.SetParent(oldSlot.transform);
            tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            tempItem.MyParent = oldSlot;
        }
        //
        // // 새 아이템 이 슬롯에 설정
        SetItem(droppedItem);
    }

    public virtual void SetItem(DraggableItem myDraggableItem)
    {
        MyDraggableItem = myDraggableItem;
        MyDraggableItem.MyParent = this;
        Debug.Log(MyDraggableItem.Id + "SetItem" + gameObject.name);
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
            // Debug.Log($"[Slot {SaveKey}] 아이템 제거됨.");
        }

        if (PlayerPrefs.HasKey(SaveKey))
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            Debug.Log($"[Slot {SaveKey}] 저장 데이터 삭제됨.");
        }
    }

    protected void Load()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!");

        if (PlayerPrefs.HasKey(SaveKey))
        {
            Debug.Log("??????????????????????????????");

            string json = PlayerPrefs.GetString(SaveKey);
            CharacterSlotItemData loadSlotItemData = JsonUtility.FromJson<CharacterSlotItemData>(json);

            GameObject newItem = Instantiate(ItemPrefab, transform);

            if (newItem.TryGetComponent(out PortraitItem item))
            {
                item.Load(loadSlotItemData.MyDraggableItemID);
            }
            else if (newItem.TryGetComponent(out EquipmentGenerator equipmentGenerator))
            {
                equipmentGenerator.Init("3");
            }
            Debug.Log(loadSlotItemData.MyDraggableItemID);
            MyDraggableItem = newItem.GetComponent<DraggableItem>();
            SetItem(MyDraggableItem);

        }
    }
}
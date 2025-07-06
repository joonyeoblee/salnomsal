using System;
using Portrait;
using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
        public bool TeamSlot;
        private void Awake()
        {
            SaveKey = "CharacterSlot_" + SlotItemID + transform.GetSiblingIndex();
        }
        private void Start()
        {
            if (TeamSlot)
            {
                GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
                if (!GameManager.Instance.IsAlive[transform.GetSiblingIndex()])
                {
                    DeleteItem(true);
                }
            }
            Load();
        }
        public override void SetItem(DraggableItem portraitItem)
        {
            if (portraitItem == null)
            {
                Debug.LogWarning("SetItem: portraitItem is null");
                return;
            }

            MyDraggableItem = portraitItem;
            MyDraggableItemID = portraitItem.Id;
            MyDraggableItem.transform.SetParent(transform);
            MyDraggableItem.transform.localPosition = DraggedSlot.localPosition;

            portraitItem.IsInSlot = true;
            base.SetItem(portraitItem);
            Debug.Log("Calling Save()");
            Save();

            if (TeamSlot)
            {
                Debug.Log(transform.GetSiblingIndex());
                PortraitItem myPortraitItem = MyDraggableItem.GetComponent<PortraitItem>();
                GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = myPortraitItem;
                GameManager.Instance.Characters[transform.GetSiblingIndex()] = myPortraitItem.portrait.Character;
                GameManager.Instance.CharacterStats[transform.GetSiblingIndex()] = myPortraitItem.SaveData.CharacterStat;
            }
        }

        public override void DeleteItem(bool destroyObject = false)
        {
            base.DeleteItem(destroyObject);
            if (TeamSlot)
            {
                GameManager.Instance.Characters[transform.GetSiblingIndex()] = null;
                GameManager.Instance.CharacterStats[transform.GetSiblingIndex()] = null;
            }
        }

        public void Save()
        {
            if (MyDraggableItem == null)
            {
                Debug.LogWarning("Save skipped: MyDraggableItem is null");
                return;
            }

            Debug.Log("Saving slot with key: " + SaveKey + ", id: " + MyDraggableItem.Id);
    
            CharacterSlotItemData slotItemData = new CharacterSlotItemData();
            slotItemData.MyDraggableItemID = MyDraggableItem.Id;
            slotItemData.SaveKey = SaveKey;

            string data = JsonUtility.ToJson(slotItemData);
            PlayerPrefs.SetString(SaveKey, data);
            PlayerPrefs.Save();

            Debug.Log("Saved JSON: " + data);
        }


      
    }
}
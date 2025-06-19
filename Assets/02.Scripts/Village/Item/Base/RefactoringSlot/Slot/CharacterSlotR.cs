using Portrait;
using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
        public bool TeamSlot;
        
        private void Start()
        {
            SaveKey = "CharacterSlot_" + SlotItemID + transform.GetSiblingIndex();

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
            if (portraitItem == null) return;
            MyDraggableItem = portraitItem;
            MyDraggableItemID = portraitItem.Id;
            MyDraggableItem.transform.SetParent(transform);
            MyDraggableItem.transform.localPosition = DraggedSlot.localPosition;

            portraitItem.IsInSlot = true;
            base.SetItem(portraitItem);
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

      
    }
}
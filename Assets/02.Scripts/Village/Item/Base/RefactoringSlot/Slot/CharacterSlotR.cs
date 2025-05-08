using Portrait;
using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
        public bool TeamSlot;

        protected override void Start()
        {
            if (TeamSlot)
            {
                GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
            }
            base.Start();
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
                GameManager.Instance.Characters[transform.GetSiblingIndex()] = myPortraitItem.portrait.Character;
                GameManager.Instance.CharacterStats[transform.GetSiblingIndex()] = myPortraitItem.SaveData.CharacterStat;
            }
        }
    }
}
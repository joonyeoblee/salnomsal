using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
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
        }
    }
}
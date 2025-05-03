using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
        public override void SetItem(DraggableItem portraitItem)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            if (portraitItem == null) return;

            _myDraggableItem = portraitItem;
            MyDraggableItemID = portraitItem.Id;
            _myDraggableItem.transform.SetParent(transform);
            _myDraggableItem.transform.localPosition = DraggedSlot.localPosition;

            portraitItem.IsInSlot = true;
            base.SetItem(portraitItem);
            Save();
        }
    }
}
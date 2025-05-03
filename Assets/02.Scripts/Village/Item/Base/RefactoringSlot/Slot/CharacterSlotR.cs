using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class CharacterSlotR : SlotR
    {
        public override void SetItem(DraggableItem portraitItem)
        {
            if (portraitItem == null) return;

            _myDraggableItem = portraitItem;
            _myDraggableItem.transform.SetParent(transform);
            _myDraggableItem.transform.localPosition = DraggedSlot.localPosition;

            portraitItem.IsInSlot = true;
            
        }
    }
}
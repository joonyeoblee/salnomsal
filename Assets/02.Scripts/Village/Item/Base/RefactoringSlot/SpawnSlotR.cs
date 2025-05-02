using Portrait;
using UnityEngine;

namespace Equipment.RefactoringSlot
{
    public class SpawnSlotR : SlotR
    {
        [SerializeField] private GameObject _currentCharacterPortrait;
        
        public void SetItem(PortraitItem portraitItem)
        {
            if (portraitItem == null) return;

            _currentCharacterPortrait = portraitItem.gameObject;
            _currentCharacterPortrait.transform.SetParent(transform);
            _currentCharacterPortrait.transform.localPosition = DraggedSlot.localPosition;
            
            portraitItem.IsInSlot = true;
            
        }
    }
}
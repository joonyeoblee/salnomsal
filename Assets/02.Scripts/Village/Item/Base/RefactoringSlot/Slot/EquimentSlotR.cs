namespace Equipment.RefactoringSlot
{
    public class EquimentSlotR : SlotR
    {
        public void LoadEquiment(string id)
        {
            MyDraggableItemID = id;
            Load();
        }
    }
}

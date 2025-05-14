namespace Equipment.RefactoringSlot
{
    public class EquimentSlotR : SlotR
    {
        public void LoadEquiment(string id)
        {
            if (id == null) return;
            MyDraggableItemID = id;
            SaveKey = "Item_" + id;
            Load();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public enum EInventoryType
{
    Team,
    Spawn,
    Inventory,
    
    Count
}
public class InventoryDDD
{
    // 지금 내가 어떤 인벤토리인가
    public EInventoryType InventoryType;
    // 지금 인벤토리가 가지고 있는 슬롯들 
    public List<SlotDDD> Slots;

    public InventoryDDD(EInventoryType inventoryType, List<SlotDDD> slots)
    {
        InventoryType = inventoryType;
        Slots = slots;
    }

    public void InitInventory(int index)
    {
        Slots = new List<SlotDDD>();
        for (int i = 0; i < index; i++)
        {
            SlotDDD slot = new SlotDDD(null, i, InventoryType.ToString());
            Slots.Add(slot);
        }
    }
    public void SetSlot(PortraitItemData portraitItemData, int index)
    { 
        Slots[index].SlotPortraitItemData = portraitItemData;
    }

    public void DeleteSlot(int index)
    {
        Slots[index].SlotPortraitItemData = null;
    }
}

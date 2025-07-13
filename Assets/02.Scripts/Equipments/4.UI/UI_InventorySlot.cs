using UnityEngine;
using Equipment;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private UI_EquipmentItem _item;
    public int Index { get; set; }
    public void Refresh(EquipmentInstance instance)
    {
        _item.Initialize(instance, this);
    }
}
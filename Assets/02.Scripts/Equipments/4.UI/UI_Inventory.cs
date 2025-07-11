using System;
using Equipment;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private UI_InventorySlot[] slots;

    private void Start()
    {
        EquipmentManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        var instances = EquipmentManager.Instance.EquipmentInstances;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < instances.Length)
                slots[i].Refresh(instances[i]);
            else
                slots[i].Refresh(null); // 빈 슬롯
        }
    }
}
using System;
using Equipment;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private UI_InventorySlot[] slots;
    public static  UI_Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }

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
            slots[i].Index = i;
            if (i < instances.Length)
                slots[i].Refresh(instances[i]);
            else
                slots[i].Refresh(null);
        }
    }
    
    public void RebuildInstancesAndSave()
    {
        EquipmentInstance[] newInstances = new EquipmentInstance[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            var item = slots[i].GetComponentInChildren<UI_EquipmentItem>();
            newInstances[i] = item != null ? item.EquipmentItemInstance : null;
        }

        EquipmentManager.Instance.EquipmentInstances = newInstances;
        EquipmentManager.Instance.Save();
    }
}
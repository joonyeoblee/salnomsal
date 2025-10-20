using System;
using Equipment;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private UI_InventorySlot[] slots;

    private void Start()
    {
        // Null safety check for EquipmentManager singleton
        if (EquipmentManager.Instance == null)
        {
            Debug.LogError("EquipmentManager.Instance is null! Make sure EquipmentManager exists in the scene.");
            return;
        }

        EquipmentManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (EquipmentManager.Instance != null)
        {
            EquipmentManager.Instance.OnDataChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        // Additional safety checks
        if (EquipmentManager.Instance == null)
        {
            Debug.LogWarning("EquipmentManager.Instance is null during Refresh");
            return;
        }

        var instances = EquipmentManager.Instance.EquipmentInstances;

        if (instances == null)
        {
            Debug.LogWarning("EquipmentInstances is null, initializing empty slots");
            // Clear all slots
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Refresh(null);
            }
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < instances.Length)
                slots[i].Refresh(instances[i]);
            else
                slots[i].Refresh(null); // 빈 슬롯
        }
    }
}
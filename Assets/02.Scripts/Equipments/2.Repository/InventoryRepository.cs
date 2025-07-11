using System;
using Equipment;
using UnityEngine;

public class InventoryRepository
{
    private const string SAVE_KEY = "Equipments";

    public void Save(EquipmentSaveData[] equipmentSaveDatas)
    {
        string json = JsonHelper.ToJson(equipmentSaveDatas);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public EquipmentSaveData[] Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return Array.Empty<EquipmentSaveData>();

        string json = PlayerPrefs.GetString(SAVE_KEY);
        return JsonHelper.FromJson<EquipmentSaveData>(json);
    }
}
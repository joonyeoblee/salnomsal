using UnityEngine;
using System.Collections.Generic;

namespace Equipment
{
    public class InventoryRepository
    {
        private const string Key = "Equipments";

        public void Save(EquipmentSaveData[] saveDatas)
        {
            string json = JsonUtility.ToJson(new EquipmentSaveWrapper { Items = saveDatas });
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }

        public EquipmentSaveData[] Load()
        {
            if (!PlayerPrefs.HasKey(Key)) return new EquipmentSaveData[0];

            string json = PlayerPrefs.GetString(Key);
            var wrapper = JsonUtility.FromJson<EquipmentSaveWrapper>(json);
            return wrapper?.Items ?? new EquipmentSaveData[0];
        }

        [System.Serializable]
        private class EquipmentSaveWrapper
        {
            public EquipmentSaveData[] Items;
        }
    }
}
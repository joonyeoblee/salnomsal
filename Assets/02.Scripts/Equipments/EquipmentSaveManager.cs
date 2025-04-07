using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public static class EquipmentSaveManager
    {
        private const string SaveKey = "SavedEquipments";

        [Serializable]
        private class EquipmentSaveDataList
        {
            public List<EquipmentSaveData> Equipments;

            public EquipmentSaveDataList(List<EquipmentSaveData> equipments)
            {
                Equipments = equipments;
            }
        }

        public static void SaveAll(List<EquipmentInstance> instances)
        {
            List<EquipmentSaveData> saveList = new();
            foreach (EquipmentInstance instance in instances)
                saveList.Add(new EquipmentSaveData(instance));

            var json = JsonUtility.ToJson(new EquipmentSaveDataList(saveList));
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
            Debug.Log($"[저장 완료] 총 {saveList.Count}개의 장비를 저장했습니다.");
        }

        public static List<EquipmentInstance> LoadAll(EquipmentDatabase database)
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                Debug.Log("[불러오기] 저장된 장비 없음");
                return new List<EquipmentInstance>();
            }

            var json = PlayerPrefs.GetString(SaveKey);
            EquipmentSaveDataList data = JsonUtility.FromJson<EquipmentSaveDataList>(json);

            List<EquipmentInstance> result = new();
            foreach (EquipmentSaveData saveData in data.Equipments)
            {
                EquipmentSO so = database.GetEquipmentByName(saveData.EquipmentSOName);
                if (so == null)
                {
                    Debug.LogError($"[복원 실패] EquipmentSO를 찾을 수 없습니다: {saveData.EquipmentSOName}");
                    continue;
                }

                EquipmentInstance instance = new(so, saveData.BaseStats, saveData.AppliedPassives)
                {
                    Id = saveData.Id // 기존 ID 유지
                };

                result.Add(instance);
            }

            Debug.Log($"[불러오기 완료] 총 {result.Count}개의 장비를 복원했습니다.");
            return result;
        }
    }
}
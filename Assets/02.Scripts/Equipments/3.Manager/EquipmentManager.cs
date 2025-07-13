using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        public EquipmentInstance[] EquipmentInstances { get; set; }

        private InventoryRepository _repository;

        public event Action OnDataChanged;

        private const int InventorySize = 20; // 고정 크기

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            EquipmentInstances = new EquipmentInstance[InventorySize];
        }

        private void Start()
        {
            _repository = new InventoryRepository();
            Load();
        }

        public void AddItem(EquipmentInstance item)
        {
            for (int i = 0; i < EquipmentInstances.Length; i++)
            {
                if (EquipmentInstances[i] == null)
                {
                    EquipmentInstances[i] = item;
                    Save();
                    OnDataChanged?.Invoke();
                    return;
                }
            }

            Debug.LogWarning("인벤토리가 가득 찼습니다.");
        }

        public void Load()
        {
            EquipmentSaveData[] saveDatas = _repository.Load();
            EquipmentInstances = new EquipmentInstance[InventorySize];

            for (int i = 0; i < saveDatas.Length && i < InventorySize; i++)
            {
                if (saveDatas[i] != null)
                    EquipmentInstances[i] = EquipmentInstance.FromSaveData(saveDatas[i]);
                else
                    EquipmentInstances[i] = null;
            }

            OnDataChanged?.Invoke();
        }

        public void Save()
        {
            EquipmentSaveData[] saveArray = new EquipmentSaveData[InventorySize];

            for (int i = 0; i < EquipmentInstances.Length; i++)
            {
                if (EquipmentInstances[i] != null)
                    saveArray[i] = EquipmentInstances[i].ToSaveData();
                else
                    saveArray[i] = null;
            }

            _repository.Save(saveArray); // null 포함된 배열 저장
        }

        public void DropItem()
        {
            
        }
    }
}

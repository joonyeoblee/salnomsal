using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        public EquipmentInstance[] EquipmentInstances { get; private set; }

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
                EquipmentInstances[i] = EquipmentInstance.FromSaveData(saveDatas[i]);
            }

            OnDataChanged?.Invoke();
        }

        public void Save()
        {
            List<EquipmentSaveData> saveList = new();
            foreach (var item in EquipmentInstances)
            {
                if (item != null)
                    saveList.Add(item.ToSaveData());
            }

            _repository.Save(saveList.ToArray());
        }

        public void DropItem()
        {
            
        }
    }
}

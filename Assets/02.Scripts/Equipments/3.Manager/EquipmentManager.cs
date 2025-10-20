using System;
using Equipment;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    public EquipmentInstance[] EquipmentInstances { get; private set; }

    [SerializeField] private EquipmentSO[] equipmentSOs;

    private InventoryRepository _repository;

    public event Action OnDataChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize in Awake to ensure data is ready before other Start() calls
        _repository = new InventoryRepository();
        Load();
    }

    public void Load()
    {
        EquipmentSaveData[] saveDatas = _repository.Load();

        // Initialize to empty array if no data
        if (saveDatas == null || saveDatas.Length == 0)
        {
            EquipmentInstances = new EquipmentInstance[0];
            Debug.Log("No equipment data found, initialized with empty array");
            OnDataChanged?.Invoke();
            return;
        }

        EquipmentInstances = new EquipmentInstance[saveDatas.Length];
        for (int i = 0; i < saveDatas.Length; i++)
        {
            EquipmentSO so = FindEquipmentSO(saveDatas[i].Id);
            if (so != null)
                EquipmentInstances[i] = saveDatas[i].ToInstance(so);
            else
                Debug.LogWarning($"SO not found for ID: {saveDatas[i].Id}");
        }

        OnDataChanged?.Invoke();
    }

    private EquipmentSO FindEquipmentSO(string id)
    {
        if (equipmentSOs == null || equipmentSOs.Length == 0)
        {
            Debug.LogWarning("equipmentSOs array is empty or null!");
            return null;
        }

        foreach (EquipmentSO so in equipmentSOs)
        {
            // Use ItemName as the unique identifier
            if (so != null && so.ItemName == id)
                return so;
        }

        return null;
    }
}
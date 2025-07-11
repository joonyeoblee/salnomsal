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
    }

    private void Start()
    {
        _repository = new InventoryRepository();
        Load();
    }

    public void Load()
    {
        EquipmentSaveData[] saveDatas = _repository.Load();

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
        // foreach (EquipmentSO so in equipmentSOs)
        // {
        //     if (so.Id == id)
        //         return so;
        // }

        return null;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 이 자식이 가지고 있어야 할 것
    // 인벤토리 리스트
    private List<InventoryDDD> _inventoryList;
    public List<InventoryDDD> InventoryList => _inventoryList;
    
    // 저장 이벤트 ( UI랑, Save용)
    public event Action OnDataChanged;
    
    // 이 자식이 해야할 일
    private void Start()
    {
        
    }
    // 지금 있는 모든 인벤토리안에 있는 슬롯들의 정보들을 모두다 로드해줘
    // 만약에 없다면 => 내가 현재 인벤토리 정보들을 가지고 인벤토리를 새로 만들어
    private void Init()
    {   
        _inventoryList = new List<InventoryDDD>();
    }
    
    // 어떤 인벤토리에 몇번 슬롯에 넣어줘 <= OnDrop으로 넣을 예정 or respawn버튼으로
    public void SetSlotPortraitItemData(EInventoryType inventoryType,int index,PortraitItemData data)
    {
        foreach (InventoryDDD inventory in _inventoryList)
        {
            if (inventory.InventoryType != inventoryType)
            {
                continue;
            }
            
            inventory.SetSlot(data, index);
        }
    }
    // 어떤 인벤토리에 몇번 슬롯에 빼줘
    public void DeleteSlotPortraitItemData(EInventoryType inventoryType, int index)
    {
        foreach (InventoryDDD inventory in _inventoryList)
        {
            if (inventory.InventoryType != inventoryType)
            {
                continue;
            }
            
            inventory.DeleteSlot(index);
        }
    }
    
    // 지금 이 인벤토리에 있는 거 저장해줘
    
    // 리스폰 시키기
}

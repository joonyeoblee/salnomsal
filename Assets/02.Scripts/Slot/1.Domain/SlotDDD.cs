using Portrait;

public class SlotDDD
{ 
    // 현재 슬롯에 가지고 있는 데이터들 
    public PortraitItemData SlotPortraitItemData;
    public PortraitItem SlotPortraitItem;
    // 지금 슬롯의 index, 아이디
    //지금 현재 슬롯의 index
    public readonly int Index;
    // 이 슬롯이 어떤 인벤토리의 슬롯인가?
    public readonly string Id;

    public SlotDDD(PortraitItemData item, int index, string id)
    {
        SlotPortraitItemData = item;
        Index = index;
        Id = id;
        
    }
    
    // 현재 슬롯의 Id, index에 있는 아이템 데이터
    public void SetData(PortraitItemData item)
    {
        SlotPortraitItemData = item;
    }
}

//     // 아이템의 ID, 이 아이템의 타입이 무엇인지, 이름까지 가지고 옴
//     public ItemData ItemData;
//     // 현재 초상화의 닉네임
//     public string PortraitName;
//     // 이 초상화의 스탯
//     public CharacterStat CharacterStat;
//     // 현재 이 캐릭터가 몇번을 클리어 했는가?
//     public int ClearCount;
//     // 현재 캐릭터가 무엇을 착용하고 있는가? 
//     public EquipmentSaveData Weapon;
//     public EquipmentSaveData EquipmentSaveData2;
//      현재 슬롯에 들어있는 캐릭터 정보들을 가지고 있는다


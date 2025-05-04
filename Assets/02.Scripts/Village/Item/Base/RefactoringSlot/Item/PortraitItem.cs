using System;
using Equipment.RefactoringSlot;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Portrait
{
    //초상화 크래스인데 이게 캐릭터 이기도 함 캐릭터별
    public class PortraitItem : DraggableItem
    {
        public PortraitSO portrait;
        int ClearCount = 0;

        Transform originalParent;
        Vector2 originalPosition;

        public float MaxHealth;
        public float MaxMana;
        public float AttackPower;
        public int Speed;
        
        public bool IsInSlot { get; set; }
        
        public void Create(string characterId) // TODO: 이름도 건네줘야함
        {
            Id = characterId;
            // name = name;

            AddRandom();
            _iconImage.sprite = portrait != null ? portrait.Icon : null;
            _itemData = new ItemData(Id, null, ItemType.Portrait);
            CharacterStat _characterStat = new CharacterStat(MaxHealth, MaxMana, AttackPower, Speed);
            SaveData = new PortraitItemData(_itemData, portrait.name, _characterStat, ClearCount);
            Save();
        }

        private void AddRandom()
        {
            MaxHealth = portrait.MaxHealth + Random.Range(0, 20);
            MaxMana = portrait.MaxMana + Random.Range(0, 2);
            AttackPower = portrait.AttackPower + Random.Range(0, 10);
            Speed = portrait.Speed + Random.Range(0, 2);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            // // 드롭된 슬롯 판단 각자 부모에서 해야할 듯
            CharacterSlotR targetSlot = eventData.pointerEnter?.GetComponentInParent<CharacterSlotR>();

            if (targetSlot != null)
            {
                // 부모를 타켓으로 수정
                MyParent = targetSlot;
                MyParent.SetItem(this);
                return;
            }

            // 슬롯에 드롭하지 않았을 경우 원래대로 복귀
            ReturnToOriginalParent();
        }
        
        // 이미 생성을 했으니 바로 저장
        private void Save()
        {
            // SaveData = new PortraitItemData(
            //     _itemData,
            //     portrait != null ? portrait.name : "",
            //     new CharacterStat(MaxHealth, MaxMana, AttackPower, Speed),
            //     ClearCount
            // );

            string json = JsonUtility.ToJson(SaveData);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }
        public void Load(string Id)
        {
            this.Id = Id;
            
            if (PlayerPrefs.HasKey(Key))
            {
                string data = PlayerPrefs.GetString(Key);
                SaveData = JsonUtility.FromJson<PortraitItemData>(data);

                // 템플릿 불러오기
                portrait = Resources.Load<PortraitSO>("Portraits/" + SaveData.PortraitName);

                if (portrait == null)
                {
                    Debug.LogWarning($"[PortraitItem] PortraitSO 로드 실패: {SaveData.ItemData.Name}");
                    return;
                }

                _iconImage.sprite = portrait.Icon;
                _itemData = SaveData.ItemData;
                MaxHealth = SaveData.CharacterStat.MaxHealth + SaveData.ClearCount;
                MaxMana = SaveData.CharacterStat.MaxMana + SaveData.ClearCount;
                AttackPower = SaveData.CharacterStat.AttackPower + SaveData.ClearCount;
                Speed = SaveData.CharacterStat.Speed + SaveData.ClearCount;

                IsInSlot = true;
            }
        }
    }
}
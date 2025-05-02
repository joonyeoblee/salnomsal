using System;
using Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Portrait
{
    [Serializable]
    public class PortraitItemData
    {
        public string CharacterId;
        public string PortraitName;
        public CharacterStat CharacterStat;
        public int ClearCount;

        public PortraitItemData(string characterId, string portraitName, CharacterStat characterStat, int clearCount)
        {
            CharacterId = characterId;
            PortraitName = portraitName;
            CharacterStat = characterStat;
            ClearCount = clearCount;
        }
    }

    //초상화 크래스인데 이게 캐릭터 이기도 함 캐릭터별
    public class PortraitItem : DraggableItem
    {
        public string CharacterId;
        public PortraitSO portrait;
        readonly string SAVE_KEY = "Character_";
        string key => SAVE_KEY + CharacterId;

        int ClearCount = 0;

        Transform originalParent;
        Vector2 originalPosition;

        public float MaxHealth;
        public float MaxMana;
        public float AttackPower;
        public int Speed;

        public PortraitItemData SaveData;
        
        public bool IsInSlot { get; set; }
        
        public void Init(string characterId)
        {
            CharacterId = characterId;
            LoadOrGenerateStat();
        }

        void LoadOrGenerateStat()
        {
            if (PlayerPrefs.HasKey(key))
            {
                Load();
            } else
            {
                AddRandom();
                _iconImage.sprite = portrait != null ? portrait.Icon : null;
                CharacterStat _characterStat = new CharacterStat(MaxHealth, MaxMana, AttackPower, Speed);
                SaveData = new PortraitItemData(CharacterId, portrait.name, _characterStat, ClearCount);
                Save();
            }
        }
        void AddRandom()
        {
            MaxHealth = portrait.MaxHealth + Random.Range(0, 20);
            MaxMana = portrait.MaxMana + Random.Range(0, 2);
            AttackPower = portrait.AttackPower + Random.Range(0, 10);
            Speed = portrait.Speed + Random.Range(0, 2);
        }
        
        
        void Save()
        {
            SaveData = new PortraitItemData(
                CharacterId,
                portrait != null ? portrait.name : "",
                new CharacterStat(MaxHealth, MaxMana, AttackPower, Speed),
                ClearCount
            );

            var json = JsonUtility.ToJson(SaveData);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        void Load()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string data = PlayerPrefs.GetString(key);
                SaveData = JsonUtility.FromJson<PortraitItemData>(data);

                // 템플릿 불러오기
                portrait = Resources.Load<PortraitSO>("Portraits/" + SaveData.PortraitName);

                if (portrait == null)
                {
                    Debug.LogWarning($"[PortraitItem] PortraitSO 로드 실패: {SaveData.PortraitName}");
                    return;
                }

                _iconImage.sprite = portrait.Icon;

                MaxHealth = SaveData.CharacterStat.MaxHealth + SaveData.ClearCount;
                MaxMana = SaveData.CharacterStat.MaxMana + SaveData.ClearCount;
                AttackPower = SaveData.CharacterStat.AttackPower + SaveData.ClearCount;
                Speed = SaveData.CharacterStat.Speed + SaveData.ClearCount; 
            }
        }
    }
}
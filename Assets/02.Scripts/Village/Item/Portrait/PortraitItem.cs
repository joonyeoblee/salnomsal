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
    public class PortraitItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public string CharacterId;
        public PortraitSO portrait;
        public CharacterSlot MyParent;
        public CharacterSlot OldParent;
        readonly string SAVE_KEY = "Character_";
        string key => SAVE_KEY + CharacterId;

        int ClearCount = 0;
        Canvas canvas;
        CanvasGroup canvasGroup;
        RectTransform rectTransform;

        Transform originalParent;
        Vector2 originalPosition;

        public float MaxHealth;
        public float MaxMana;
        public float AttackPower;
        public int Speed;

        public PortraitItemData SaveData;

        [SerializeField] Image _iconImage;
        public bool IsInSlot { get; set; }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
            _iconImage = GetComponent<Image>();
        }

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
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsInSlot)
            {
                OldParent = MyParent;
                MyParent.DeleteItem(); // 기존 슬롯에서 제거
            }

            originalParent = transform.parent;
            originalPosition = rectTransform.anchoredPosition;

            transform.SetParent(canvas.transform);
            canvasGroup.blocksRaycasts = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            // 드롭된 슬롯 판단
            CharacterSlot targetSlot = eventData.pointerEnter?.GetComponentInParent<CharacterSlot>();

            if (targetSlot != null)
            {
                // 슬롯이 유효하면 슬롯이 처리하도록 위임 (슬롯에서 SetItem 또는 ChangeSlot 실행됨)
                return;
            }

            // 슬롯에 드롭하지 않았을 경우 원래대로 복귀
            ReturnToOriginalParent();
        }
        void ReturnToOriginalParent()
        {
            MyParent = OldParent;

            if (MyParent != null)
            {
                MyParent.SetItem(this); // 슬롯의 currentCharacterPortrait도 복구됨
                transform.SetParent(MyParent.transform); // 부모 복구
                transform.localPosition = MyParent.DraggedSlot.localPosition; // 위치 복구
                IsInSlot = true;
            } else
            {
                // 슬롯이 아닌 곳에서 드래그된 경우 처리
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = originalPosition;
                IsInSlot = false;
            }
        }

        
        public void StartManualDrag()
        {
            originalParent = transform;
            originalPosition = GetComponent<RectTransform>().anchoredPosition;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsLastSibling(); // 맨 앞으로
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
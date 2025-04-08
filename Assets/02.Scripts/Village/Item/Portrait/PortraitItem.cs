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
        public PortraitSO Portrait;
        public CharacterStat CharacterStat;

        public PortraitItemData(string characterId, PortraitSO portrait, CharacterStat characterStat)
        {
            CharacterId = characterId;
            Portrait = portrait;
            CharacterStat = characterStat;
        }
    }

    //초상화 크래스인데 이게 캐릭터 이기도 함 캐릭터별
    public class PortraitItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public string CharacterId;
        public PortraitSO portrait;
        public CharacterSlot MyParent;
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

        PortraitItemData _saveData;

        Image _iconImage;
        public bool IsInSlot { get; set; }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
            _iconImage = GetComponent<Image>();
        }

        // void Start()
        // {
        //     Init("1");
        // }
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
                CharacterStat _characterStat = new CharacterStat(MaxHealth, MaxMana, AttackPower);
                _saveData = new PortraitItemData(CharacterId, portrait, _characterStat);
                Save();
            }
        }
        void AddRandom()
        {
            MaxHealth = portrait.MaxHealth + Random.Range(1, 5); // 1~4
            MaxMana = portrait.MaxMana + Random.Range(1, 5); // 1~4
            AttackPower = portrait.AttackPower + Random.Range(1, 5); // 1~4
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsInSlot)
            {
                MyParent.DeleteItem();

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
            Slot targetSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();

            if (targetSlot == null)
            {
                // 슬롯이 아닌 곳 → 삭제
                Debug.Log("슬롯 외부로 드롭됨 → 삭제");

                // Destroy(gameObject);
            }

            IsInSlot = true;

            // 슬롯에 잘 드롭됨 → 처리 생략 (OnDrop이 자동으로 실행됨)
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
            string json = JsonUtility.ToJson(_saveData);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        void Load()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string data = PlayerPrefs.GetString(key);
                _saveData = JsonUtility.FromJson<PortraitItemData>(data);
                portrait = _saveData.Portrait;
                _iconImage.sprite = portrait.Icon;
                MaxHealth = _saveData.CharacterStat.MaxHealth;
                MaxMana = _saveData.CharacterStat.MaxMana;
                AttackPower = _saveData.CharacterStat.AttackPower;
            }
        }
    }
}
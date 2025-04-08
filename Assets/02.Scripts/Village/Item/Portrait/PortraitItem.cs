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
        public int ClearCount;

        public PortraitItemData(string characterId, PortraitSO portrait, CharacterStat characterStat, int clearCount)
        {
            CharacterId = characterId;
            Portrait = portrait;
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
                _saveData = new PortraitItemData(CharacterId, portrait, _characterStat, ClearCount);
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

            // 드롭된 슬롯 판단은 CharacterSlot에서 처리하도록 위임
            CharacterSlot targetSlot = eventData.pointerEnter?.GetComponentInParent<CharacterSlot>();

            if (targetSlot == null)
            {
                ReturnToOriginalParent();
            }

            // 슬롯이 유효하면 아무것도 하지 않고, CharacterSlot.OnDrop()에서 처리
        }
        void ReturnToOriginalParent()
        {
            // 부모 복원
            transform.SetParent(originalParent, true);

            // 위치 복원
            rectTransform.anchoredPosition = originalPosition;
        }

        void ChangeParent()
        {

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
                MaxHealth = _saveData.CharacterStat.MaxHealth + _saveData.ClearCount;
                MaxMana = _saveData.CharacterStat.MaxMana + _saveData.ClearCount;
                AttackPower = _saveData.CharacterStat.AttackPower + _saveData.ClearCount;
                Speed = _saveData.CharacterStat.Speed + _saveData.ClearCount; 
            }
        }
    }
}
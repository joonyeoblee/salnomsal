using System;
using Portrait;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    [Serializable]
    public class CharacterSlotItemData
    {
        public string id;
    }
    public class CharacterSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] RectTransform _draggedSlot;
        [SerializeField] public GameObject currentCharacterPortrait;
        [SerializeField] Canvas canvas;
        [SerializeField] GameObject _itemPrefab;
        CharacterSlotItemData _slotItemData;
        string SaveKey;
        void Start()
        {
            SaveKey = "CharaceterSlot_" + transform.GetSiblingIndex();
            Load();
        }
        public void SetItem(PortraitItem portraitItem)
        {
            currentCharacterPortrait = portraitItem.gameObject;
            currentCharacterPortrait.transform.SetParent(transform);
            currentCharacterPortrait.transform.localPosition = _draggedSlot.localPosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (currentCharacterPortrait != null)
            {
                return;
            }

            PortraitItem portraitItem = eventData.pointerDrag.GetComponent<PortraitItem>();
            portraitItem.IsInSlot = true;
            portraitItem.MyParent = this;
            if (portraitItem != null)
            {
                SetItem(portraitItem);
                Save();

                // Destroy(dragItem.gameObject);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (currentCharacterPortrait == null)
            {
                return;
            }

            // 드래그 오브젝트 가져오기
            PortraitItem drag = currentCharacterPortrait.GetComponent<PortraitItem>();
            if (drag == null)
            {
                return;
            }

            // 슬롯에서 제거 (자식에서 분리)
            currentCharacterPortrait.transform.SetParent(canvas.transform);
            Debug.Log("드래그 시작 전 currentCharacterPortrait: " + currentCharacterPortrait?.name);
            currentCharacterPortrait = null;
            Debug.Log("드래그 후 currentCharacterPortrait: " + currentCharacterPortrait?.name);

            // 드래그 시작
            // drag.StartManualDrag();


        }


        void Save()
        {
            if (currentCharacterPortrait != null)
            {
                // DragItem에서 ID 꺼내기
                PortraitItem item = currentCharacterPortrait.GetComponent<PortraitItem>();
                if (item != null && item.CharacterId != null)
                {
                    CharacterSlotItemData slotItemData = new CharacterSlotItemData();
                    slotItemData.id = item.CharacterId;
                    string data = JsonUtility.ToJson(slotItemData);
                    PlayerPrefs.SetString(SaveKey, data);
                    PlayerPrefs.Save();
                    Debug.Log($"[Slot {SaveKey}] 저장됨: {item.CharacterId}");
                }
            }
        }

        void Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                _slotItemData = JsonUtility.FromJson<CharacterSlotItemData>(json);
                Debug.Log($"[Slot {SaveKey}] 불러옴: {_slotItemData.id}");

                // 아이템 프리팹을 인스턴스화해서 슬롯에 배치
                GameObject newItem = Instantiate(_itemPrefab, transform);
                newItem.transform.localPosition = _draggedSlot.localPosition;

                // 해당하는 플레이어 소환해서 붙여야함
                PortraitItem portraitItem = newItem.GetComponent<PortraitItem>();
                portraitItem.MyParent = this;
                portraitItem.IsInSlot = true;
                portraitItem.Init(_slotItemData.id);
                currentCharacterPortrait = newItem;
            }
        }

        public void DeleteItem()
        {
            if (currentCharacterPortrait != null)
            {
                currentCharacterPortrait = null; // 참조도 제거
                Debug.Log($"[Slot {SaveKey}] 아이템 제거됨.");
            }

            if (PlayerPrefs.HasKey(SaveKey))
            {
                PlayerPrefs.DeleteKey(SaveKey); // 저장된 데이터도 제거
                PlayerPrefs.Save();
                Debug.Log($"[Slot {SaveKey}] 저장 데이터 삭제됨.");
            }
        }
    }
}
